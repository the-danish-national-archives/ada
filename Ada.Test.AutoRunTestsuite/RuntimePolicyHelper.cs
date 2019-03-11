namespace Ada.Test.AutoRunTestsuite
{
    #region Namespace Using

    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using NUnit.Framework;

    #endregion

    /// <summary>
    ///     This class is need as an alternative to setting the "useLegacyV2RuntimeActivationPolicy="true" in app.config.
    ///     Whcih can not be set in a test project
    /// </summary>
    [SetUpFixture]
    public class RuntimePolicyHelper
    {
        #region Properties

        public static COMException LegacyV2RuntimeEnabledException { get; private set; }
        public static bool LegacyV2RuntimeEnabledSuccessfully { get; private set; }

        #endregion

        #region

        [Test]
        public void CheckPolicySetCorrectly()
        {
            if (LegacyV2RuntimeEnabledException != null) throw LegacyV2RuntimeEnabledException;
            Assert.IsTrue(LegacyV2RuntimeEnabledSuccessfully);
        }

        [OneTimeSetUp]
        public static void SetPolicy()
        {
            var clrRuntimeInfo =
                (ICLRRuntimeInfo)
                RuntimeEnvironment.GetRuntimeInterfaceAsObject(Guid.Empty, typeof(ICLRRuntimeInfo).GUID);

            // Allow errors to propagate so as to fail the tests.
            clrRuntimeInfo.BindAsLegacyV2Runtime();

            try
            {
                clrRuntimeInfo.BindAsLegacyV2Runtime();
                LegacyV2RuntimeEnabledSuccessfully = true;
            }
            catch (COMException ex)
            {
                // This occurs with an HRESULT meaning 
                // "A different runtime was already bound to the legacy CLR version 2 activation policy."
                LegacyV2RuntimeEnabledSuccessfully = false;
                LegacyV2RuntimeEnabledException = ex;
            }
        }

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