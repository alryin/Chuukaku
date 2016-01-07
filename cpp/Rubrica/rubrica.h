/*
version 1.0

Written by Ryick

(add your name and modify the version number if you update the file)
*/

#ifndef RUBRICA_H
#define RUBRICA_H

#include <string>
#include <ostream>
#include <stdexcept>
#include "voce.h"

class rubrica{
	
	public:
	
	typedef unsigned int size;
	
	rubrica();
	explicit rubrica(size _maxSize);
	rubrica(const rubrica &other);
	~rubrica();
	
	void set_capacity(size _newMaxSize);
	
	size contained() const; //deve funzionare anche per le rubriche costanti
	size maxSize() const;
	const voce& operator[](size i) const;
	
	rubrica& operator= (rubrica &r); // !
	
	void add(const string nome, const string cognome, const string ntel);
	void add(const voce &other);
	
	const voce& find(string& ntel) const;
	
	void clear();
	
	void save(const std::string filename) const;
	
	void load(const std::string filename);
	
	void rubrica::swapR(rubrica &other);

private:
	
	voce* _voci;
	size _size; // numero di elementi attuali
	size _maxSize; // numero di elementi massimi
	
	//friend std::ostream &operator<<(std::ostream &os, const rubrica &r);
	
};

ostream& operator<< (ostream& os, const rubrica& v);

//eccezioni
class rubrica_piena : public std::runtime_error {
public:
	rubrica_piena() : runtime_error("Err: rubrica piena") {}
};

class voce_esistente : public std::runtime_error {
public:
	voce_esistente() : runtime_error("Err: voce gia' presente") {}
};

class voce_inesistente : public std::runtime_error {
public:
	voce_inesistente() : runtime_error("Err: voce inesistente") {}
};
//eof eccezioni

#endif
//eof rubrica.h
