using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User{
	private string username;
	private string password;
	private PlayerStatus status;
	private string profile;

	public User(string name, string pw){
		username = name;
		password = pw;
	}

	
    public string getUsername()
    {
        return username;
    }
}
