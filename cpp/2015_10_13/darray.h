#ifndef DARRAY_H
#define DARRAY_H
 
class darray {
	private: //ridondante: non serve in quanto se non specificato è private di default
		typedef unsigned int size_type; // perché sì: mappa il tipo di dato in un nome.
	
		int *ptr;
		size_type size; // numero di interi puntati da ptr. unsigned per non avere valori negativi.	

	public: //tutto quello scritto da qui in poi è pubblico
			/* il primo metodo fondamentale da implementare sempre è il costruttore di default.
			   In caso contrario ne viene "implementato" automaticamente come in Java
			   ma è saggio costruirlo.*/
		
		darray(void);
		/* in C se non viene specificato niente si assume come 
		   parametro in input un intero (non in C++ ma non si sa mai). */
		
		//costruttore con paramentri in input
		darray(size_type size);
		
		/* 
			2° metodo fondametale: distruttore
			pattern RAII https://en.wikipedia.org/wiki/Resource_Acquisition_Is_Initialization 
		*/
		~/* alt+126 */darray(void);
		
		int get_value(size_type i);
		
}; // la sintassi di chiusura è uguale alle strutture.

#endif
