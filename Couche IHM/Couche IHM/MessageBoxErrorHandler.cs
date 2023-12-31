using Couche_IHM.VueModeles;
using GalliumPlusApi.Exceptions;
using System;
using System.Windows;

namespace Couche_IHM
{
    public static class MessageBoxErrorHandler
    {
        public static Action<T> AttachToAction<T>(Action<T> action)
        {
            return input => DoesntThrow(() => action(input));
        }

        public static bool DoesntThrow(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (AggregateException aggregateErrors)
            {
                if (aggregateErrors.InnerException != null)
                {
                    HandleError(aggregateErrors.InnerException);
                    return false;
                }
                return true;
            }
            catch (Exception error)
            {
                HandleError(error);
                return false;
            }
        }

        private static void HandleError(Exception error)
        {
            switch (error)
            {
                case UnauthenticatedException:
                    MessageBox.Show("Veuillez vous reconnecter.", "Session expirée", MessageBoxButton.OK, MessageBoxImage.Error);
                    MainWindowViewModel.Instance.MainWindow.AskToDisconnect();
                    break;

                default:
                    MessageBox.Show(error.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
    }
}
