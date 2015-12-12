/*
version 1.0

Written by Ryick 

(add your name and modify the version number if you modify the file)
*/
#include "voce.h"
#include <string>
#include <ostream>

using namespace std;

struct voce {
	
	voce() {
		this->nome = "";
		this->cognome = "";
		this->ntel = "";
	}

	voce(string nome, string cognome, string ntel) {
		this->nome = nome;
		this->cognome = cognome;
		this->ntel = ntel;
	}

	ostream& operator<< (ostream& os, const voce& v) {
		os << v.nome << " " << v.cognome << " " << v.ntel;
		return os;
	}

}
//eof voce.cpp
