namespace Ada.Test.UnitTests.Actions
{
    #region Namespace Using

    using System;
    using System.Linq;
    using Action64.IngestActions;
    using ActionBase;
    using Ada.Actions;
    using Ada.Actions.TestActions;
    using Checks;
    using NUnit.Framework;

    #endregion

    [TestFixture(Category = "AdaActionContainer")]
    internal class AdaActionContainerTest
    {
        [TestCase]
        public void AdaActionContainerTestSimpleLoad()
        {
            // Arrange
            AdaActionContainer.Instance.Clear();
            // Act
            // Assert
            Assert.DoesNotThrow(() => AdaActionContainer.Instance.Load<DocumentsOtherIngestAction>());
            Assert.DoesNotThrow(() => AdaActionContainer.Instance.Load<AdaSingleQueryAction>());
        }

        [TestCase]
        public void AdaActionContainerTestSimpleLoadTwiceGood()
        {
            // Arrange
            AdaActionContainer.Instance.Clear();
            AdaActionContainer.Instance.Load<DocumentsOtherIngestAction>();
            AdaActionContainer.Instance.Load<AdaSingleQueryAction>();
            // Act
            // Assert
            Assert.DoesNotThrow(() => AdaActionContainer.Instance.Load<AdaSingleQueryAction>(false));
        }

        [TestCase]
        public void AdaActionContainerTestSimpleLoadTwiceBad()
        {
            // Arrange
            AdaActionContainer.Instance.Clear();
            AdaActionContainer.Instance.Load<DocumentsOtherIngestAction>();
            AdaActionContainer.Instance.Load<AdaSingleQueryAction>();
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => AdaActionContainer.Instance.Load<AdaSingleQueryAction>(true));
        }


        [TestCase]
        public void AdaActionContainerTestSimpleGetAction()
        {
            // Arrange
            AdaActionContainer.Instance.Clear();
            AdaActionContainer.Instance.Load<DocumentsOtherIngestAction>();
            AdaActionContainer.Instance.Load<AdaSingleQueryAction>();

            // Act
            var temp = AdaActionContainer.Instance.GetActions(typeof(DiskSpaceWarning));

            // Assert
            Assert.AreEqual(temp.SingleOrDefault(), typeof(AvailableDiskSpaceTestAction));
        }
    }
}