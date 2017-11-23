﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    /// <summary>
    /// Represents a collection of generic items that are categorized.
    /// </summary>
    public class CategorizedItemCollection
    {
        #region Properties
        /// <summary>
        /// Gets or sets the categories for the collection.
        /// </summary>
        public List<Category> Categories { get; set; }
        #endregion

        #region Constructors
        public CategorizedItemCollection()
        {
            this.Categories = new List<Category>();
        }
        #endregion

        #region Category
        /// <summary>
        /// Represents a category in a CategorizedItemCollection
        /// </summary>
        public class Category
        {
            #region Properties
            /// <summary>
            /// Gets or sets the Name of the category
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the Items for the category
            /// </summary>
            public List<Item> Items { get; set; }
            #endregion

            #region Constructors
            public Category()
            {
                this.Items = new List<Item>();
            }
            #endregion

            #region Item
            /// <summary>
            /// Represents an Item in a Category
            /// </summary>
            public class Item
            {
                #region Properties
                /// <summary>
                /// Gets or sets the name for the item
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// Gets or sets a description for the item.
                /// </summary>
                public string Description { get; set; }

                /// <summary>
                /// Gets or sets binary data for an icon for the item.
                /// </summary>
                public byte[] Icon { get; set; }

                /// <summary>
                /// Gets or sets meta data for the item that will be passed from the client to the server when an item is selected.
                /// This meta data can be used to construct a new instance of an object for the collection.
                /// </summary>
                public string MetaData { get; set; }
                #endregion
            }
            #endregion
        }
        #endregion
    }
}