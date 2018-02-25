using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour {
	private string username;
	private string password;
	private Enums.PlayerStatus status;
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
	
}
