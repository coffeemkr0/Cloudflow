//Example of modal placeholder markup
//<div id='myModal' class='modal fade in'>
//    <div class="modal-dialog">
//        <div class="modal-content">
//            <div id='myModalContent'></div>
//        </div>
//    </div>
//</div>

$(function () {
    $("a[data-modal]").on("click", function (e) {
        var $target = $(e.target);
        var modalContainerId = $target.attr("data-modalcontainer")
        var modalContentId = $target.attr("data-modalcontent")
        $("#" + modalContentId).load(this.href, function () {
            $("#" + modalContainerId).modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');

            bindForm(this, modalContainerId, modalContentId);
        });

        return false;
    });
});

function bindForm(dialog, modalContainerId, modalContentId) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $("#" + modalContainerId).modal('hide');
                    //Refresh
                    location.reload();
                } else {
                    $("#" + modalContentId).html(result);
                    bindForm();
                }
            },
            cache: false
        });
        return false;
    });
}