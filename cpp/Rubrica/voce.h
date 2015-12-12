/*
version 1.1

Written by Ryick

(add your name and modify the version number if you update the file)
*/

#ifndef VOCE_H
#define VOCE_H

#include <string>
#include <ostream>

using namespace std;

struct Voce {
	string nome;
	string cognome;
	string ntel;

	Voce();
	Voce(string nome, string cognome, string ntel);

};

ostream& operator<< (ostream& os, const voce& v);

#endif
//eof voce.h
