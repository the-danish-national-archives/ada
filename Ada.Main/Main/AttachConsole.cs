namespace Ada.Main
{
    #region Namespace Using

    using System.Runtime.InteropServices;

    #endregion

    public class ConsoleAttacher
    {
        #region Static

        private const int AttachParentProcess = -1;

        #endregion

        #region

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessID);

        public static bool AttachParrentProcessToConsole()
        {
            if (AttachConsole(AttachParentProcess))
                return true;


            AllocConsole();

            return false;
        }

        #endregion
    }
}