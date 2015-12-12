/*
version 1.0

Written by Ryick

(add your name and modify the version number if you modify the file)
*/
#ifndef VOCE_H
#define VOCE_H

#include <string>
#include <ostream>

using namespace std;

struct voce {
	string nome;
	string cognome;
	string ntel;

	voce();
	voce(string nome, string cognome, string ntel);

	ostream& operator<< (ostream& os, const voce& v);
};

#endif
//eof voce.h
