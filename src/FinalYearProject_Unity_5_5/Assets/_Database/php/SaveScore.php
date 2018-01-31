<?php	

	$sqlconnection = mysqli_connect('kunet','k1457777','sk8ordie1958','db_k1457777');
	if(mysqli_connect_errno())
	{
		echo"Failed to connect to database.".mysqli_connect_error();
	}
	$name = mysqli_real_escape_string($sqlconnection, $_POST['newName'] );
	$song = mysqli_real_escape_string($sqlconnection, $_POST['newSong']);
	$score = mysqli_real_escape_string($sqlconnection, $_POST['newScore'] );

	if(isset($name) && isset($song) && isset($score))
	{
		$query = "INSERT INTO scores (name,song,score) VALUES ('$name','$song','$score')";
		$result = mysqli_query($sqlconnection, $query);
		if(!$result)
		{
			echo"Failure.";
		}
		else
		{
			echo"Success.";
		}
	}
?>