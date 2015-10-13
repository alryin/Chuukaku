#include "darray.h"

int main(void /* non prende argomenti */){
	// non faccendo new alloco sullo stack e non sullo heap: 
	darray a(10); //costruito con il costruttore di default
	
	/*
	darray *b = new darray; //allocato sullo heap
	delete b;
	*/
	return 0;
}