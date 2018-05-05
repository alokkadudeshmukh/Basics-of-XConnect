<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KioskContact.aspx.cs" Inherits="KioskContact.KioskContact" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <!-- Theme Made By www.w3schools.com - No Copyright -->
    <title>HI xConnect Kiosk</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link href="https://fonts.googleapis.com/css?family=Lato" rel="stylesheet" type="text/css">
    <link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet" type="text/css">
    <link rel="shortcut icon" type="image/ico" href="images/favicon.ico">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <style>
    body {
        font: 400 15px/1.8 Lato, sans-serif;
        color: #777;
    }

    h3,
    h4 {
        margin: 10px 0 30px 0;
        letter-spacing: 10px;
        font-size: 20px;
        color: #111;
    }

    .container {
        padding: 30px 120px 0;
    }

    .person {
        border: 10px solid transparent;
        margin-bottom: 25px;
        width: 80%;
        height: 80%;
        opacity: 0.7;
    }

    .person:hover {
        border-color: #f1f1f1;
    }

    .carousel-inner img {
        width: 100%;
        /* Set width to 100% */
        margin: auto;
    }

    .carousel-caption h3 {
        color: #fff !important;
    }

    @media (max-width: 600px) {
        .carousel-caption {
            display: none;
            /* Hide the carousel text when the screen is less than 600 pixels wide */
        }
    }

    .bg-1 {
        background: #2d2d30;
        color: #bdbdbd;
    }

    .bg-1 .container {
        background: url(images/pixels-light-left.png) no-repeat top left;
    }

    .bg-2 .container {
        background: url(images/pixels-dark-right.png) no-repeat top right;
    }

    .bg-1 h3 {
        color: #fff;
    }

    .bg-1 p {
        font-style: italic;
    }

    .list-group-item:first-child {
        border-top-right-radius: 0;
        border-top-left-radius: 0;
    }

    .list-group-item:last-child {
        border-bottom-right-radius: 0;
        border-bottom-left-radius: 0;
    }

    .thumbnail {
        padding: 0 0 15px 0;
        border: none;
        border-radius: 0;
        background: none;
    }

    .thumbnail img {
        border-radius: 10%;
    }

    .thumbnail p {
        margin-top: 15px;
        color: #555;
    }

    .btn {
        padding: 10px 20px;
        background-color: #333;
        color: #f1f1f1;
        border-radius: 0;
        transition: .2s;
    }

    .btn:hover,
    .btn:focus {
        border: 1px solid #333;
        background-color: #fff;
        color: #000;
    }

    .modal-header,
    h4,
    .close {
        background-color: #333;
        color: #fff !important;
        text-align: center;
        font-size: 30px;
    }

    .modal-header,
    .modal-body {
        padding: 40px 50px;
    }

    .nav-tabs li a {
        color: #777;
    }

    #googleMap {
        width: 100%;
        height: 400px;
        -webkit-filter: grayscale(100%);
        filter: grayscale(100%);
    }

    .navbar {
        font-family: Montserrat, sans-serif;
        margin-bottom: 0;
        background-color: #2d2d30;
        border: 0;
        font-size: 11px !important;
        letter-spacing: 4px;
        opacity: 0.9;
    }

    .navbar li a,
    .navbar .navbar-brand {
        color: #d1ce56 !important;
    }

    .navbar-nav li a:hover {
        color: #fff !important;
    }

    .navbar-nav li.active a {
        color: #fff !important;
        background-color: #29292c !important;
    }

    .navbar-default .navbar-toggle {
        border-color: transparent;
    }

    .open .dropdown-toggle {
        color: #fff;
        background-color: #555 !important;
    }

    .dropdown-menu li a {
        color: #000 !important;
    }

    .dropdown-menu li a:hover {
        background-color: red !important;
    }

    footer {
        background-color: #2d2d30;
        color: #d1ce56;
        padding: 10px 32px;
    }

    footer a {
        color: #d1ce56;
    }

    footer a:hover {
        color: #777;
        text-decoration: none;
    }

    .form-control {
        border-radius: 0;
        height: 40px;
        border-radius: 5px !important;
        color: #555;
    }

    .form-control::placeholder {
        color: #555;
        opacity: 1;
        /* Firefox */
    }

    .form-control:-ms-input-placeholder {
        /* Internet Explorer 10-11 */
        color: #555;
    }

    .form-control::-ms-input-placeholder {
        /* Microsoft Edge */
        color: #555;
    }

    .input-select {
        height: 40px;
        border: 0 none;
        border-radius: 5px !important;
        width: 100%;
        background: #fff;
        border: 1px solid #ccc;

        color: #555;
    }

    textarea {
        resize: none;
    }

    #contact {
        margin-top: 50px;
    }

    .navbar-header .navbar-brand,
    .navbar-header {
        width: 100%;
        text-align: center;
    }

    #btn_capture {
        width: 100%;
        display: block;
        margin: 10px 0 20px;
    }

    #v,
    #captureimage {
        border-radius: 20px;
    }
   
    #captureimage {
        width: 100%;
        height: 100%;
    }

    .video-container {
        position: relative;
    }

    #cameraspinner {
        position: absolute;
        top: 0;
        z-index: 1;
        right: 15px;
    }
    </style>
</head>
<body id="myPage" data-spy="scroll" data-target=".navbar" data-offset="50">    
    
     <nav class="navbar navbar-default navbar-fixed-top">
        <div class="container-fluid">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="#myPage">HI xConnect Kiosk</a>
            </div>
            <!-- <div class="collapse navbar-collapse" id="myNavbar">
                <ul class="nav navbar-nav navbar-right">
                    <li><a href="#myPage">HOME</a></li>
                    <li><a href="#band">ABOUT HI</a></li>
                    <li><a href="#contact">CONTACT</a></li>
                </ul>
            </div> -->
        </div>
    </nav>
    <!-- Container (Contact Section) -->
    <div id="contact" class="bg-1">
        <form id="form1" runat="server">
        <div class="container">
            <h3 class="text-center">CONTACT</h3>
            <!-- <p class="text-center"><em>We love our fans!</em></p>
             -->
            <div class="row">
                <div class="col-md-3 video-container">
                    <video id="v" width="100%" height="100%"></video>
                    <input type="button" id="btn_capture" value="Capture" class="btn" />
                    <canvas id="c" style="display:none;" width="100%" height="100%"></canvas>
                    <img id="captureimage" runat="server" />
                    <img id="cameraspinner" src="images/camera-spinner.gif" height="35px" width="35px" />
                </div>
                <div class="col-md-9">
                    <div class="row">
                        <div class="col-sm-12 form-group">                            
                             <asp:DropDownList ID="ddTitle" runat="server" CssClass="input-select">
                                 <asp:ListItem  Text="Title" Value=""></asp:ListItem>
                        <asp:ListItem Text="Mr." Value="Mr."></asp:ListItem>
                        <asp:ListItem Text="Ms." Value="Ms."></asp:ListItem>
                    </asp:DropDownList>
                        </div>
                        <div class="col-sm-12 form-group">
                            <%--<input class="form-control" id="fname" name="fname" placeholder="First Name" type="text" required>--%>
                            <asp:TextBox class="form-control" ID="txtFName" runat="server" placeholder="First Name" ></asp:TextBox>
                        </div>
                        <div class="col-sm-12 form-group">
                            <%--<input class="form-control" id="mname" name="mname" placeholder="Middle Name" type="text" required>--%>
                            <asp:TextBox class="form-control" ID="txtMName" runat="server" placeholder="Middle Name"></asp:TextBox>
                        </div>
                        <div class="col-sm-12 form-group">
                            <%--<input class="form-control" id="lname" name="lname" placeholder="Last Name" type="text" required>--%>
                            <asp:TextBox class="form-control" ID="txtLname" runat="server" placeholder="Last Name"></asp:TextBox>
                        </div>
                        <div class="col-sm-12 form-group">                            
                             <asp:DropDownList ID="ddGender" runat="server" CssClass="input-select">
                                 <asp:ListItem Text="Gender" Value=""></asp:ListItem>
                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                    </asp:DropDownList>
                        </div>
                        <div class="col-sm-12 form-group">
                            <%--<input class="form-control" id="jtitle" name="jtitle" placeholder="Job Title" type="text" required>--%>
                             <asp:TextBox CssClass="form-control" ID="txtJobRole" runat="server" placeholder="Job Title"></asp:TextBox>
                        </div>
                        <div class="col-sm-12 form-group">
                            <%--<input class="form-control" id="email" name="email" placeholder="Email" type="email" required>--%>
                            <asp:TextBox CssClass="form-control" ID="txtEmailAddress" runat="server" placeholder="Email"></asp:TextBox>
                        </div>
                        <div class="col-sm-12 form-group">
                             <asp:DropDownList ID="ddLanguage" runat="server" CssClass="input-select">
                        <asp:ListItem Text="Prefered Language" Value=""></asp:ListItem>
                                 <asp:ListItem Text="English" Value="English"></asp:ListItem>
                        <asp:ListItem Text="Hindi" Value="Hindi"></asp:ListItem>
                        <asp:ListItem Text="French" Value="French"></asp:ListItem>
                    </asp:DropDownList>
                        </div>
                        <div class="col-xs-12 form-group error-msg">
                             <asp:Literal ID="ltrl_status" runat="server"></asp:Literal>
                    <input type="hidden" runat="server" id="hdnbase" />
                        </div>                      
                    </div>
                </div>
                <div class="col-md-12 form-group">
                    <%--<button class="btn pull-right" type="submit">Send</button>--%>
                    <asp:Button CssClass="btn pull-right" ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click" />
                </div>
                <br>
            </div>
        </div>
            </form>
    </div>
    <!-- Footer -->
    <footer class="text-center">
        <a class="up-arrow" href="#myPage" data-toggle="tooltip" title="TO TOP">
    <span class="glyphicon glyphicon-chevron-up"></span>
  </a>
        <br>
        <h3 class="text-center" style="color: #777;font-size: 15px;margin-bottom: 10px;">CRAFTS MEN</h3>
        <p style="color:#777;margin: 0;">Alok KaduDeshmukh - BED</p>
        <p style="color:#777;margin: 0;">Chirayu Sharma - FED</p>
        <p style="color:#777;margin: 0;">Pranav Geria - FED</p>
        <br/>
        <p>&copy;2018 Horizontal Integration</p>
    </footer>
   

     <script>
    $(document).ready(function() {
        // Initialize Tooltip
        $('[data-toggle="tooltip"]').tooltip();

        // Add smooth scrolling to all links in navbar + footer link
        $(".navbar a, footer a[href='#myPage']").on('click', function(event) {

            // Make sure this.hash has a value before overriding default behavior
            if (this.hash !== "") {

                // Prevent default anchor click behavior
                event.preventDefault();

                // Store hash
                var hash = this.hash;

                // Using jQuery's animate() method to add smooth page scroll
                // The optional number (900) specifies the number of milliseconds it takes to scroll to the specified area
                $('html, body').animate({
                    scrollTop: $(hash).offset().top
                }, 900, function() {

                    // Add hash (#) to URL when done scrolling (default click behavior)
                    window.location.hash = hash;
                });
            } // End if
        });
        navigator.getUserMedia({ video: true }, function(stream) {
            var video = document.getElementById("v");
            var canvas = document.getElementById("c");
            var button = document.getElementById("btn_capture");
            //video.src = stream;
            video.srcObject = stream;
            video.play();
            button.disabled = false;
            var scale = 0.25;
            button.onclick = function() {
                canvas.width = video.videoWidth * scale;
                canvas.height = video.videoHeight * scale;
                canvas.getContext("2d").drawImage(video, 0, 0, canvas.width, canvas.height);
                var img = canvas.toDataURL("image/png");
                var pageUrl = '/KioskContact.aspx';
                document.getElementById("captureimage").src = img;
                //document.getElementById("captureimage").setAttribute("dataurl", img);
                document.getElementById("hdnbase").value = img.replace(/^data:image\/(png|jpg);base64,/, "");
                //alert(document.getElementById("hdnbase").value);
                $.ajax({
                    type: "POST",
                    url: pageUrl + "/GetCapturedImage",
                    data: "{ 'basestring':'" + document.getElementById("hdnbase").value + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(response) {
                        //alert(response.d);
                    },
                    failure: function(response) {
                        //alert(response.d);
                    }
                });
            };
        }, function(err) { alert("there was an error " + err) });


    });
    </script>
</body>
</html>
