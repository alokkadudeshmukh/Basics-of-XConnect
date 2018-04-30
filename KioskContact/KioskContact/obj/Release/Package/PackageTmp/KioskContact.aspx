<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KioskContact.aspx.cs" Inherits="KioskContact.KioskContact" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <%--START SWF WebCam--%>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<%--<script src='<%=ResolveUrl("~/Webcam_Plugin/jquery.webcam.js") %>' type="text/javascript"></script>
<script type="text/javascript">
var pageUrl = '<%=ResolveUrl("~/KioskContact.aspx") %>';
$(function () {
    jQuery("#webcam").webcam({
        width: 320,
        height: 240,
        mode: "save",
        swffile: '<%=ResolveUrl("~/Webcam_Plugin/jscam.swf") %>',
        debug: function (type, status) {
            $('#camStatus').append(type + ": " + status + '<br /><br />');
        },
        onSave: function (data) {
            $.ajax({
                type: "POST",
                url: pageUrl + "/GetCapturedImage",
                data: '',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    $("[id*=imgCapture]").css("visibility", "visible");
                    $("[id*=imgCapture]").attr("src", r.d);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        },
        onCapture: function () {
            webcam.save(pageUrl);
        }
    });
});
function Capture() {
    webcam.capture();
    return false;
}
</script>--%>
    <%--END SWF WEBCAM--%>
    <form id="form1" runat="server">
        <div>
            <h1>Welcome to Kiosk App</h1>
            <table>
                <tr>
                    <td>Title:</td>
                    <td><asp:DropDownList ID="ddTitle" runat="server">
<asp:ListItem Text="Mr." Value="Mr."></asp:ListItem>
                        <asp:ListItem Text="Ms." Value="Ms."></asp:ListItem>
                        </asp:DropDownList> </td>
                </tr>
                <tr>
                    <td>First Name:</td>
                    <td><asp:TextBox ID="txtFName" runat="server" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Middle Name:</td>
                    <td><asp:TextBox ID="txtMName" runat="server" ></asp:TextBox></td>
                </tr>
                 <tr>
                    <td>Last Name:</td>
                    <td><asp:TextBox ID="txtLname" runat="server" ></asp:TextBox></td>
                </tr>

                 <tr>
                    <td>Email Address:</td>
                    <td><asp:TextBox ID="txtEmailAddress" runat="server" ></asp:TextBox></td>
                </tr>
                  <tr>
                    <td>Preferred Language:</td>
                    <td><asp:DropDownList ID="ddLanguage" runat="server">
<asp:ListItem Text="English" Value="English"></asp:ListItem>
                        <asp:ListItem Text="Hindi" Value="Hindi"></asp:ListItem>
                        <asp:ListItem Text="French" Value="French"></asp:ListItem>
                        </asp:DropDownList> </td>
                </tr>
                <%--START SWF WEBCAM--%>
              <%--  <tr>
        <td>
            <div id="webcam">
            </div>
        </td>
        
        <td>
            <asp:Image ID="imgCapture" runat="server" Style="visibility: hidden; width: 320px;
                height: 240px" />
        </td>
    </tr>
                  <tr>
        <td align="center">
            <u>Live Camera</u>
        </td>
        <td>
        </td>
        <td align="center">
            <u>Captured Picture</u>
        </td>
    </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnCapture" Text="Capture" runat="server" OnClientClick="return Capture();" />
<br />
<span id="camStatus"></span>
                    </td>
                </tr>--%>
               <%-- END SWF WEBCAM--%>
                <tr>
                    <td colspan="2"><asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click" /> </td>
                </tr>
                 <tr>
                    <td colspan="2">
                        <asp:Label ID="lbl_status" runat="server"></asp:Label>
                        <input type="hidden" runat="server" id="hdnbase" />
                    </td>
                     </tr>
            </table>
              <video id="v" width="300" height="300"></video>
            <%--<asp:Button ID="btn_capture" runat="server" Text="Capture" ClientIDMode="Static" OnClick="btn_capture_Click" />--%>
            <input type="button" id="btn_capture" value="Capture" />
	<canvas id="c" style="display:none;" width="300" height="300"></canvas>
            <%--<canvas id="c" style="display:none;"></canvas>--%>
    <img id="captureimage" runat="server" />
      <script type="text/javascript">
        navigator.getUserMedia({video: true}, function(stream) {
		var video = document.getElementById("v");
		var canvas = document.getElementById("c");
		var button = document.getElementById("btn_capture");
		//video.src = stream;
            video.srcObject = stream;
      video.play();
          button.disabled = false;
          var scale = 0.25;
          button.onclick = function () {
              canvas.width = video.videoWidth * scale;
              canvas.height = video.videoHeight * scale;
              canvas.getContext("2d").drawImage(video, 0, 0, canvas.width, canvas.height);
			var img = canvas.toDataURL("image/png");
            var pageUrl = '<%=ResolveUrl("~/KioskContact.aspx") %>';
            document.getElementById("captureimage").src = img;
            //document.getElementById("captureimage").setAttribute("dataurl", img);
            document.getElementById("hdnbase").value = img.replace(/^data:image\/(png|jpg);base64,/, "");
           // alert(document.getElementById("hdnbase").value);
            $.ajax({
                type: "POST",
                url: pageUrl + "/GetCapturedImage",
                data: "{ 'basestring':'"+ document.getElementById("hdnbase").value+"'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert(response.d);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
		};
	}, function(err) { alert("there was an error " + err)});
    </script>
        </div>
    </form>
</body>
</html>

<asp:ListItem Text="Mr." Value="Mr."></asp:ListItem>