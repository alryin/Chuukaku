/*
version 2.0

Written by Ryick 

(add your name and modify the version number if you update the file)
*/
#include "voce.h"

using namespace std;

//more efficient than initialization
voce() : 
	nome(), cognome(), ntel() { }

voce(string nome, string cognome, string ntel): 
	nome(nome), cognome(cognome), ntel(ntel) { }

ostream& operator<< (ostream& os, const voce& v) {
	os << v.nome << " " << v.cognome << " " << v.ntel;
	return os;
}
//eof voce.cpp
