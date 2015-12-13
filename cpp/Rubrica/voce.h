/*
version 1.2.1

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

	Voce(void);
	Voce(string nome, string cognome, string ntel);
	~Voce(void);
};

ostream& operator<< (ostream& os, const Voce& v);

#endif
//eof voce.h
