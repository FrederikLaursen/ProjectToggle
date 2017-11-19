<?php
require_once('/usr/share/php/libphp-phpmailer/PHPMailerAutoload.php');
include_once("/usr/share/php/libphp-phpmailer/phpmailer/class.phpmailer.php");

ini_set('display_errors', 'On');
error_reporting(E_ALL);

	$servername = "localhost";
	$username = "root";
	$password = "unitydb123";
	$dbname = "Unityproject";

	$conn = new PDO("mysql:host=$servername;dbname=$dbname", $username, $password);
	
	$username = $_POST['username'];
	echo $username;
	$sql = "SELECT `ID` FROM `Users` WHERE username = '$username'";     // Setup the query
	$query = $conn->query($sql); 						    		// Run query in db
	$result = $query->fetch(PDO::FETCH_ASSOC);  		    		// Fetch the data
	$id = $result['ID']; 		
	
	$randomPass = bin2hex(openssl_random_pseudo_bytes(4));
	$hashpass = password_hash($randomPass, PASSWORD_DEFAULT);
	$sql = "UPDATE `Users` SET `password`='$hashpass' WHERE ID = '$id'";   // Setup the query
	$query = $conn->query($sql); 						   			   	   // Run query in db

	$sql = "SELECT email FROM Users WHERE ID = '$id'";     // Setup the query
	$query = $conn->query($sql); 						   // Run query in db
	$result = $query->fetch(PDO::FETCH_ASSOC);  		   // Fetch the data
	$toEmail = $result['email']; 						   // Echo the username
	

	$mail = new PHPMailer(); // create a new object
	$mail->IsSMTP(); // enable SMTP
	$mail->SMTPDebug = 1; // debugging: 1 = errors and messages, 2 = messages only
	$mail->SMTPAuth = true; // authentication enabled
	$mail->SMTPSecure = 'ssl'; // secure transfer enabled REQUIRED for Gmail
	$mail->Host = "smtp.gmail.com";
	$mail->Port = 465;
	$mail->IsHTML(true);
	$mail->Username = "simonfjoensson@gmail.com";
	$mail->Password = "x612M^44R8zb";
	$mail->SetFrom("simonfjoensson@gmail.com");
	$mail->Subject = "Password reset";

	// Generate a new random password for the user
	// Set their password to the temp password
	// Generate a link to resetpassword with their ID
	$mail->Body = "Follow the link to reset your password using this temporary code: $randomPass. https://www.projecttoggle.dk/reset.html?$id";
	echo $toEmail;
	$mail->AddAddress($toEmail);
	
	if(!$mail->Send()) {
	   echo "Mailer Error: " . $mail->ErrorInfo;
	} else {
	   echo "Message has been sent" . $randomPass;
	}
	$conn = null;	
?>