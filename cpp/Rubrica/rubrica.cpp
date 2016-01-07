/*

version 1.0

Written by Ryick

(add your name and modify the version number if you update the file)
*/

#include "rubrica.h"
#include <algorithm>
#include <cassert>
#include <fstream>

typedef unsigned int size;

using namespace std;
	
rubrica::rubrica() : _voci(0), _size(0), _maxSize(0) {/*nop*/};
	
rubrica::rubrica(size maxSize) : _voci(0), _size(0), _maxSize(0) {
	_voci = new voce[maxSize];
	_maxSize = maxSize;
	
};
	
rubrica& rubrica::operator= (rubrica &r) {
	if (this != &r){
		rubrica tmp(r);
		
		swapR(tmp);
	} 
	
	return *this;
};
	
rubrica::rubrica(const rubrica &other) : _voci(0), _size(0), _maxSize(0) {
	try{
		for (int i = 0; i < other._size; i++){
			_voci[i] = other._voci[i];
		}
		_maxSize = other._maxSize;
		_size = other._size;
	} catch(...){
		delete [] _voci;
		_maxSize = 0;
		_size = 0;
		throw;
	}
};

rubrica::~rubrica(){
	delete [] _voci;
	_size = 0;
	_maxSize = 0;
	_voci = 0;
};
	
void rubrica::set_capacity(size _newMaxSize){
	if (_newMaxSize == _maxSize) return;
	
	rubrica tmp(_newMaxSize);
	
	if (_newMaxSize > _size)
		try{
			for (int i = 0; i < _size; i++){ 
				tmp._voci[i] = this -> _voci[i];
			}
		} catch(...){
			delete [] tmp._voci;
			tmp._size = 0;
			tmp._maxSize = 0;
			tmp._voci = 0; throw;
		}
	
	swapR(tmp);

	return;
};
	
size rubrica::contained() const {
	return _size;
}; //deve funzionare anche per le rubriche costanti
	
size rubrica::maxSize() const {
	return _maxSize;
};
	
const voce& rubrica::operator[](size i) const {
	//mi assicuro che i sia una voce esistente
	assert(i < _size);
	return _voci[i];
};
	
void rubrica::add(const string nome, const string cognome, const string ntel) {
	add(voce(nome, cognome, ntel)); return;
};
	
void rubrica::add(const voce &other) {
	if (this -> _size == this -> _maxSize)
		throw rubrica_piena();
	
	for (int i = 0; i < _size; i++){
		if (other.ntel == _voci[i].ntel) throw voce_esistente();
	}	
		
	_voci[_size] = other;
	_size++;
	return;
};
	
const voce& rubrica::find(string& ntel) const {
	for (int pieroangela = 0; pieroangela < this -> _size; pieroangela++)
		if (_voci[pieroangela].ntel == ntel)
			return _voci[pieroangela];
	
	throw voce_inesistente();
	};
	
void rubrica::clear() {
	delete [] _voci;
	_size = 0;
	return;
};

//preso dal codice del prof (visionato solo a scopo didattico)
void rubrica::save(const std::string filename) const {
	ofstream ofs(filename.c_str());
	
	// Memorizzo nel file PRIMA la capacita' perche' serve per poter
	// allocate subito l'array che conterra' le voci
	ofs << _maxSize << endl;
	
	// Salvo il numero di voci
	ofs << _size << endl;
		
	// Ciclo per salvare le voci
	// Ogni termine e' separato da uno spazio (IMPORTANTE)
	for(int i = 0 ; i < _size; i++)
		ofs << _voci[i].cognome << " " << _voci[i].nome << " " <<_voci[i].ntel << endl;
	
	return;
};

//preso dal codice del prof (visionato solo a scopo didattico)
void rubrica::load(const std::string filename) {
	
	ifstream ifs(filename.c_str());
	
	size mS;
	size cned;
	
	string cognome, nome, ntel;
	
	// Leggo la capacita' e il numero di voci 
	ifs >> mS >> cned;

	// alloco l'array di voci
	rubrica tmp(mS);

	// Ciclo per leggere le voci un termine alla volta
	// legge una stringa fino ad uno spazio o /n
	for (int i = 0; i < cned; i++) {
		ifs >> cognome >> nome >> ntel;
		tmp.add(cognome, nome, ntel);
	}
	
	swapR(tmp);
	return;
};

/*
riscrivo la funzione swap per essere adatta allo scambio tra rubriche
in quanto ricorrente nel codice
*/
void rubrica::swapR(rubrica &other) {
	swap(_voci, other._voci);
	swap(_maxSize, other._maxSize);
	swap(_size, other._size);	
}

ostream& operator<< (ostream& os, const rubrica& v) {
	for (int i = 0; i < v.contained(); i++) { os << v[i] << endl; } 
	return os;
};
		
//eof rubrica.h
