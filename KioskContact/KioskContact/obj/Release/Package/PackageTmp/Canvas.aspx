<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Canvas.aspx.cs" Inherits="KioskContact.Canvas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>    
   <video id="v" width="300" height="300"></video>
	<input id="b" type="button" disabled="true" value="Take Picture" />
	<canvas id="c" style="display:none;" width="300" height="300"></canvas>
    <img id="captureimage" runat="server" />
      <script type="text/javascript">
        navigator.getUserMedia({video: true}, function(stream) {
		var video = document.getElementById("v");
		var canvas = document.getElementById("c");
		var button = document.getElementById("b");
		//video.src = stream;
            video.srcObject = stream;
      video.play();
		button.disabled = false;
		button.onclick = function() {
			canvas.getContext("2d").drawImage(video, 0, 0, 300, 300, 0, 0, 300, 300);
			var img = canvas.toDataURL("image/png");
            alert("done");
            document.getElementById("captureimage").src = img;
		};
	}, function(err) { alert("there was an error " + err)});
    </script>
</body>
  
</html>
