    function newuserclick() {
        $.ajax({
            url: '/Home/NewProfile',
            success: function (data) {
                document.getElementById('profileImage').src = '/Images/Portrait.png';
                $("#renderBody").html(data);
                $("#usersList tr").removeClass('active');
                $("#usersList table").prepend('<tr id="usersList0"><td style="cursor: pointer;"><img src="/Images/Portrait.png" class="littlePortrait" /></td><td>New User</td></tr>');
            },
            error: function (xhr, textStatus, error) {
                console.log(xhr.statusText);
                console.log(textStatus);
                console.log(error);
            }
        })
    }

    $('#search').on('input', function (e) {
        filtering();
    });

    $("#enabled").click(function () {
        $("#disabled").css("font-weight", "unset");
        $("#enabled").css("font-weight", "bold");
        filtering();
    });

    $("#disabled").click(function () {
        $("#disabled").css("font-weight", "bold");
        $("#enabled").css("font-weight", "unset");
        filtering();
    });

    function filtering() {
        var elemsCount = document.getElementById("asideList").getElementsByTagName("li").length;
        var searchText = document.getElementById("search").value;
        var disabledFontWeight = $("#disabled").css("font-weight");
        var enabledFontWeight = $("#enabled").css("font-weight");
        var boldest = disabledFontWeight > enabledFontWeight ? "Disabled" : "Enabled";
        $("#asideBar").load('@Url.Action("Searching", "Home")', {text: searchText, bold:boldest,elemscount: elemsCount});
    }

    $("#profileImageDiv").click(function () {
        document.getElementById('file-input').click();
    });

    $(document).ready(function () {
        document.getElementById('userRoleLink').addEventListener('click', function (e) {
            checkErrorsOnPage(e);
        }, false);
        document.getElementById('userSettingsLink').addEventListener('click', function (e) { checkErrorsOnPage(e); }, false);
});

    function AjaxError(jqXHR, exception) {
        var msg = '';
        if (jqXHR.status === 0) {
            msg = 'Not connect.\n Verify Network.';
        } else if (jqXHR.status == 404) {
            msg = 'Requested page not found. [404]';
        } else if (jqXHR.status == 500) {
            msg = 'Internal Server Error [500].';
        } else if (exception === 'parsererror') {
            msg = 'Requested JSON parse failed.';
        } else if (exception === 'timeout') {
            msg = 'Time out error.';
        } else if (exception === 'abort') {
            msg = 'Ajax request aborted.';
        } else {
            msg = 'Uncaught Error.\n' + jqXHR.responseText;
        }
        console.log(msg);
    }

    
      
    
