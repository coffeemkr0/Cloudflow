$(function () {
    $(document).on("click", ".extensionBrowser__addButton", function () {
        var $clickedItemElement = $(this).closest(".extensionBrowser__item");
        var $extensionBrowserElement = $clickedItemElement.closest(".extensionBrowser");
        var collectionId = $extensionBrowserElement.attr("data-collectionid");
        var $extensionCollectionElement = $("#" + collectionId);

        var extensionAssemblyPath = $clickedItemElement.find("input[name='extensionAssemblyPath']").val();
        var extensionId = $clickedItemElement.find("input[name='extensionId']").val();

        AddExtension($extensionCollectionElement, extensionId, extensionAssemblyPath);
    });
});

function AddExtension(extensionCollectionElement, extensionId, extensionAssemblyPath) {
    $.ajax({
        type: 'POST',
        url: "/Jobs/AddConfigurableExtension",
        dataType: 'json',
        data: {
            extensionId: extensionId,
            extensionAssemblyPath: extensionAssemblyPath,
            extensionType: extensionCollectionElement.attr("data-extensiontype"),
            propertyName: extensionCollectionElement.attr("data-propertyname")
        },
        success: function (result) {
            if (result !== null) {
                extensionCollectionElement.find(".objectCollectionEdit__navigationItems").first().append(result.navigationItem);
                extensionCollectionElement.find(".objectCollectionEdit__items").first().append(result.itemEdit);
                UpdateObjectCollectionNames(extensionCollectionElement.find(".objectCollectionEdit__navigationItems").first());
            } else {
                alert('Error getting data.');
            }
        },
        error: function () {
            alert('Error getting data.');
        }
    });
}