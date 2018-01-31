<?php
	$servername = "kunet";
	$username = "k1457777";
	$password = "sk8ordie1958";
	$DBName = "db_k1457777";
	
	//Make connection
	$conn = new mysqli($servername,$username,$password,$DBName);
	//Check connection
	if(!$conn)
	{
		die("Connection failed. ".mysqli_connect_error());
	}
	
	$sql = "SELECT name, song, score FROM scores ORDER BY score DESC LIMIT 5";
	$result = mysqli_query($conn,$sql);
	
	if(mysqli_num_rows($result)>0)
	{
		//Show data for each row
		while($row = mysqli_fetch_assoc($result))
		{
			echo "| Name: ".$row['name']."|Song: ".$row['song']. "| Score: ".$row['score']. ";" ;
		}
	}
?>