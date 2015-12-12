/*
version 2.1

Written by Ryick 

(add your name and modify the version number if you update the file)
*/

#include "voce.h"

using namespace std;
//implementation of voce.h methods

//more efficient than initialization
Voce::Voce() : 
	nome(), cognome(), ntel() { }

Voce::Voce(string nome, string cognome, string ntel): 
	nome(nome), cognome(cognome), ntel(ntel) { }

ostream& operator<< (ostream& os, const Voce& v) {
	os << v.nome << " " << v.cognome << " " << v.ntel;
	return os;
}
//eof voce.cpp
