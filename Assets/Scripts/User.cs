using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User{
	private string username;
	private string password;
	private PlayerStatus status;
	private string profile;
	private Role curRole;

	public User(string name, string pw){
		username = name;
		password = pw;
	}

	public void setRole(Role role){
		curRole = role;
	}

	public Role getRole(){
		return curRole;
	}
	
    public string getUsername()
    {
        return username;
    }
}
