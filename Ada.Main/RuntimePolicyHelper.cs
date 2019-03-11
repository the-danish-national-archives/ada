namespace Ada
{
    #region Namespace Using

    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>
    ///     This class is need as an alternative to setting the "useLegacyV2RuntimeActivationPolicy="true" in app.config.
    ///     Whcih can not be set in a test project
    /// </summary>
    public static class RuntimePolicyHelper
    {
        #region  Constructors

        static RuntimePolicyHelper()
        {
            var clrRuntimeInfo =
                (ICLRRuntimeInfo) RuntimeEnvironment.GetRuntimeInterfaceAsObject(
                    Guid.Empty,
                    typeof(ICLRRuntimeInfo).GUID);
            try
            {
                clrRuntimeInfo.BindAsLegacyV2Runtime();
                LegacyV2RuntimeEnabledSuccessfully = true;
            }
            catch (COMException)
            {
                // This occurs with an HRESULT meaning 
                // "A different runtime was already bound to the legacy CLR version 2 activation policy."
                LegacyV2RuntimeEnabledSuccessfully = false;
            }
        }

        #endregion

        #region Properties

        public static bool LegacyV2RuntimeEnabledSuccessfully { get; }

        #endregion

        #region Nested type: ICLRRuntimeInfo

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("BD39D1D2-BA2F-486A-89B0-B4B0CB466891")]
        private interface ICLRRuntimeInfo
        {
            void xGetVersionString();
            void xGetRuntimeDirectory();
            void xIsLoaded();
            void xIsLoadable();
            void xLoadErrorString();
            void xLoadLibrary();
            void xGetProcAddress();
            void xGetInterface();
            void xSetDefaultStartupFlags();
            void xGetDefaultStartupFlags();

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void BindAsLegacyV2Runtime();
        }

        #endregion
    }
}