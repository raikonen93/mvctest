var page = '';


function newuserclick() {
        $.ajax({
            url: '/Home/NewProfile',
            success: function (data) {
                document.getElementById('profileImage').src = '/Images/Portrait.png';                
                $("#renderBody").html(data);
                $("#usersList tr").removeClass('active');
                $("#usersList table").prepend('<tr style="cursor:pointer" id="usersList0"><td style="cursor: pointer;"><img src="/Images/Portrait.png" class="littlePortrait" /></td><td>New User</td></tr>');
            },
            error: function (jqXHR, exception) { AjaxError(jqXHR, exception); }
        })
    }


       

function checkErrorsOnPage(e) {
        focusOut();
        if (document.getElementById('namespan').style.visibility == "visible" || document.getElementById('emailspan').style.visibility == "visible") {
            e.preventDefault();
        }
}

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

$(document).ready(function () {      

        $("#profileImageDiv").click(function () {
            document.getElementById('file-input').click();
        });

        document.getElementById('userRoleLink').addEventListener('click', function (e) {
            checkErrorsOnPage(e);
        }, false);

        document.getElementById('userSettingsLink').addEventListener('click', function (e) { checkErrorsOnPage(e); }, false);
        $('#search').on('input', function (e) {
            filtering();
        });

        $("#enabled").click(function () {
            $("#disabled").css("font-weight", "unset");
            $("#enabled").css("font-weight", "bold");
            setCookie("bold", "Enabled");   
            filtering(false);
        });

        $("#disabled").click(function () {
            $("#disabled").css("font-weight", "bold");
            $("#enabled").css("font-weight", "unset");
            setCookie("bold", "Disabled");   
            filtering(false);
        });
});

function GetCurrentPage() {
    return document.getElementById('Page').value; 
}

function setCookie(key, value) {
    var expires = new Date();
    expires.setTime(expires.getTime() + (1 * 24 * 60 * 60 * 1000));
    document.cookie = key + '=' + value + ';expires=' + expires.toUTCString();
}

function getCookieValue(a) {
    var b = document.cookie.match('(^|[^;]+)\\s*' + a + '\\s*=\\s*([^;]+)');
    return b ? b.pop() : '';
}


    
      
    
