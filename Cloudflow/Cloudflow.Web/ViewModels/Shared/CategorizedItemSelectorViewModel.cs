using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Shared
{
    public class CategorizedItemSelectorViewModel
    {
        #region Properties
        public Guid Id { get; set; }

        public CategorizedItemSelectorAttribute.CategorizedItemCollection ItemCollection { get; set; }

        public string Caption { get; set; }

        public string CategoriesCaption { get; set; }
        #endregion

        #region Constructors
        public CategorizedItemSelectorViewModel()
        {

        }
        #endregion
    }
}