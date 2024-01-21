using CommunityToolkit.Maui.Core.Primitives;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace CopyBag.Models.CopyBin
{
    [Table("CopyItems")]
    public class CopyItem : BaseClass
    {
        public string TitleItemCopy { get; set; }
        public string TitleOne { get; set; }
        public string CopyTextOne { get; set; }
        public string TitleTwo { get; set; }
        public string CopyTextTwo { get; set; }
        public bool IsTwin { get; set; }

        [ForeignKey(typeof(CopyFolder))]
        public int FolderId { get; set; }

        [ManyToOne]
        public CopyFolder CopyFolder { get; set; }

        //public int CategoryId { get; private set; }
        //public CopyCategory CopyCategory { get; private set; }
        public CopyItem()
        {
        }

        public CopyItem(string titleCopyItem, string titleOne, string copyTextOne, string titleTwo, string copyTextTwo, int folderId)
        {
            TitleItemCopy = titleCopyItem;
            TitleOne = titleOne;
            CopyTextOne = copyTextOne;
            if (string.IsNullOrWhiteSpace(copyTextTwo))
            {
                TitleTwo = string.Empty;
                CopyTextTwo = string.Empty;
                IsTwin = false;
            }
            else
            {
                TitleTwo = titleTwo;
                CopyTextTwo = copyTextTwo;
                IsTwin = true;
            }
            FolderId = folderId;
        }

        public void Edit(string titleCopyItem, string titleOne, string copyTextOne, string titleTwo, string copyTextTwo)
        {
            TitleItemCopy = titleCopyItem;
            TitleOne = titleOne;
            CopyTextOne = copyTextOne;
            if (string.IsNullOrWhiteSpace(copyTextTwo))
            {
                TitleTwo = string.Empty;
                CopyTextTwo = string.Empty;
                IsTwin = false;
            }
            else
            {
                TitleTwo = titleTwo;
                CopyTextTwo = copyTextTwo;
                IsTwin = true;
            }
            LastSeen = DateTime.Now;
        }
    }
}
