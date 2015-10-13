#include "darray.h"
#include <iostream>
#include <cassert>

darray::darray(void){
	#ifndef NDEBUG
		std::cout << "darray::darray()" << std::endl;
	#endif
			
	this->ptr = 0; // puntatore a se stesso: stessa funzione di this in Java.
	this->size = 0;	
}

darray::darray(size_type size){
	#ifndef NDEBUG // -DNDEBUG sul promt per definire NDEBUG (vedi MAKEFILE)
		std::cout << "darray::darray()" << std::endl;
	#endif
	
	ptr = new int[size]; //heap
	this->size = size;
}

darray::~darray(void) {
	// non chiamare distruttore in modo espicito: viene chiamato dal compilatore.
	delete[] ptr;
	this -> ptr = 0; // bene mettere puntatore a null per sicurezza
	this -> size = 0;
}

// Eccezioni in C++ vengono usate solo per errori particolari e/o imprevedibili a differenza di Java
// Tutto ciò che è eseguibile con un if va usato assert
int darray::get_value(size_type i){
	assert(i < this->size); // verifica che i sia minore di size. Se fallisce il programma termina. Viene mandato messaggio esplicito.
	
	return this->ptr[i];
}