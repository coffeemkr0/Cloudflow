using System;
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
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the caption to display on a modal dialog for the collection.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the caption to display above the categories on a modal dialog for the collection.
        /// </summary>
        public string CategoriesCaption { get; set; }

        /// <summary>
        /// Gets or sets the categories for the collection.
        /// </summary>
        public List<Category> Categories { get; set; }
        #endregion

        #region Constructors
        public CategorizedItemCollection(Guid collectionId, string caption, string categoriesCaption)
        {
            this.Id = collectionId;
            this.Caption = categoriesCaption;
            this.CategoriesCaption = categoriesCaption;
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
                /// Gets or sets the path to the assembly that contains the object factory extension that will be
                /// used for creating an instance of the item.
                /// </summary>
                public string ObjectFactoryAssemblyPath { get; set; }

                /// <summary>
                /// Gets or sets the Id of the object factory extensions that will be used for creating an instance of the item.
                /// </summary>
                public Guid ObjectFactoryExtensionId { get; set; }

                /// <summary>
                /// Gets or sets factory data that will be used to create an object factory for creating an instance of the item.
                /// </summary>
                public string FactoryData { get; set; }

                /// <summary>
                /// Gets or sets instance data that will be used by an object factory to initialize an instance of the item.
                /// </summary>
                public string InstanceData { get; set; }
                #endregion
            }
            #endregion
        }
        #endregion
    }
}
