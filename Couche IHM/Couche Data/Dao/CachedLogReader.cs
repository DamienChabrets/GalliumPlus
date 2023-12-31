using Couche_Data.Interfaces;
using Modeles;
using System.Threading.Tasks.Dataflow;

namespace Couche_Data.Dao
{
    public class CachedLogReader : IPaginatedLogReader
    {
        private List<Log> allLogs;
        private BufferBlock<Log> buffer;
        private int pageSize;
        private int pageIndex;
        private bool exhausted;
        private object loadLock;

        public CachedLogReader(int pageSize, List<Log> allLogs)
        {
            this.allLogs = allLogs;
            buffer = new BufferBlock<Log>();
            this.pageSize = pageSize;
            pageIndex = 0;
            exhausted = false;
            loadLock = new object();
        }

        public int PageSize => pageSize;

        public IAsyncEnumerable<Log> GetAsyncStream(CancellationToken ct = default) => buffer.ReceiveAllAsync(ct);

        public void LoadNextPage()
        {
            lock (loadLock)
            {
                if (!exhausted)
                {
                    int pageStart = pageIndex * pageSize;
                    int pageEnd = pageStart + pageSize;

                    if (pageEnd > allLogs.Count)
                    {
                        pageEnd = allLogs.Count;
                        exhausted = true;
                    }
                    else
                    {
                        pageIndex++;
                    }

                    for (int i = pageStart; i < pageEnd; i++)
                    {
                        buffer.Post(allLogs[i]);
                    }
                }
            }
        }

        public void Reset()
        {
            // désactiver l'ancien tampon et en créer un nouveau
            buffer.Complete();
            buffer = new BufferBlock<Log>();

            // remettre le curseur au début
            pageIndex = 0;
            exhausted = false;
        }
    }
}
