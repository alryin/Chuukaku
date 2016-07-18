#ifndef CBUFFER_H
#define CBUFFER_H

#include <iostream> 	//std::cout
#include <algorithm> 	//std::swap
#include <iterator> 	//std::forward_iterator_tag

/**
La classe cbuffer rappresenta un buffer circolare templato,
implementato tramite l'uso di una lista concatenata singolarmente.
Una volta decisa la dimensione del buffer, questa non può essere più modificata.
*/
template <typename T>
class cbuffer
{
	public:
	typedef unsigned int size_type; 
	
	
	/**
	La struttura node rappresenta un nodo della lista-buffer.
	**/
	struct node
	{
		T elem; 		///< Dato Templato del nodo 
		node* next; 	///< Puntatore al nodo successivo
		
		
		/**
			Costruttore che serve per costruire un nodo dato un valore.
		
			@param el valore del nodo.
		*/
		node(const T &el) : elem(el), next(0){}
		
		
		/**
		Distruttore.
		*/
		~node(){}
		
		
		/**
		Costruttore di default: crea un oggetto "inconsistente" in quanto il dato templato risulta nullo.
		*/
		node() : elem(0), next(0){}

		
		/**
		Costruttore di Copia.
		
		@param other nodo da copiare.
		*/
		node(const node &other): elem(other.elem) {}

		
		/**
		Overloading dell'operatore assegnamento.
		
		@param other nodo da copiare.
		*/
		node&operator=(const node &other)
		{ 
			if(this != other) std::swap(this.elem, other.elem);
		}
	}; //struct node
	
	
	private:
	size_type	size;			///< Dimensione massima del Buffer.
	size_type allocati;			///< Allocati è il numero di elementi allocati nel buffer, allocati <= size.
	node *testa;				///< Puntatore iniziale al Buffer.
	node *coda;					///< Puntatore all'elemento corrente del Buffer (inteso sempre come l'ultimo elemento inserito).
	
	
	public:	
	/**
	Costruttore di default.
	*/
	cbuffer(): size(0), allocati(0), testa(0), coda(0) {}
	
	
	/**
	Costruttore Secondario che crea un buffer di dimensione _size.
	
	@param _size dimensione del buffer.
	*/
	explicit cbuffer(const size_type &_size) : size(_size), allocati(0), testa(0), coda(0) {}


	/**
	Costruttore secondario che inizializza i nodi con un il valore passato in input.
	
	@param _size dimensione del buffer.
	@param el elemento di inizializzazione.
	*/

	cbuffer(const size_type &_size, const T &el) : size(_size), allocati(0), testa(0), coda(0) 
	{
		for(size_type i = 0 ; i < _size; i++) {	add(el); }
	}

	
	/**
	Distruttore della classe.
	*/
	~cbuffer() { delete_all(); }
	
	
	/**
	Overloading dell'operatore [] in scrittura/lettura.
	
	@param index indice del dato a cui si vuole accedere.
	@return T oggetto salvato alla posizione index.
	*/
	T& operator[](const size_type &index)
	{
		if(index < allocati)
		{
			node *found = testa;
			for( int i=0; i<index; i++){ found = found-> next; }
			return found -> elem;
		}
		else std::cout << "Buffer [" << index << "]: non Esiste!!" << std::endl;
	}

	
	/**
	Overloading dell'operatore [] in sola lettura.
	
	@param index indice del dato a cui si vuole accedere.
	@return T oggetto salvato alla posizione index.
	*/
	const T& operator[] (const size_type &index) const
	{
		if(index < allocati)
		{
			node *found = testa;
			for( int i=0; i<index; i++){ found = found-> next; }
			return found -> elem;
		}
		else std::cout << "Buffer [" << index << "]: non Esiste!!" << std::endl;
	}
	
	
	/**
	Costruttore di copia di un buffer.
	
	@param other buffer da copiare.
	*/
	cbuffer(const cbuffer &other): size(other.getSize()), allocati(0), testa(0), coda(0)  
	{
		for(size_type s = 0; s < other.allocati; s++ ){ add(other[s]); }
	}
	
	
	/**
	Overloading dell'operatore assegnamento(=).
	
	@param other buffer da copiare.
	*/
	void operator=(const cbuffer &other) 
	{
		if(this != &other)
		{
			delete_all();
			size = other.size;
			node *tmp = other.testa;
			for(size_type s = 0; s < other.allocati; s++ )
			{ 
				add( tmp -> elem);
				tmp = tmp -> next;	 
			}
		}
	}
	
	
	/**
	Overloading dell'operarore di assegnamento(=). 
	Il buffer passato in input, però, è di un tipo diverso 
	da  quello del chiamante; funziona solo se funziona lo static cast,
	cioè se i due tipi di dati possono essere "castati" ed da int a double.
			
	@param other cbuffer da copiare
	**/
	template<typename Q>
	void operator=(const cbuffer<Q> &other) 
	{	
		delete_all();
		size = other.getSize();
		
		for (size_type s = 0; s < other.getAllocati(); s++)
		{
			try{ add(static_cast<T>(other[s])); }
			catch(...){delete_all(); throw;}
		}
	}

			
	/**
	Implementazione della funzione che aggiunge un elemento al buffer.
	Qualora il buffer sia pieno, viene sovrascritto l'elemento più vecchio.
	
	@param other elemento templato da aggiungere.
	*/	
	void add(const T &other)
	{	
		if(testa == 0 && coda == 0)
		{
			try
			{
				testa = new node(other);
				coda = testa;
				allocati++;
			}
			catch(...) { delete_all(); throw;}
		}
		else if(allocati <= size-1){ add_helper(other); }
		else if(allocati == size)
		{
			coda -> next = testa;
			testa = testa-> next;
			coda = coda -> next;
			coda -> elem = other;
			coda -> next = 0;		
		}
	}
	
	
	/**
	Funzione ausiliaria che viene utilizzata per l'add.
	
	@param other elemento da aggiungere.
	*/
	void add_helper(const T &other)
	{
		try
		{	
			node *tmp = coda;
			coda = new node(other);
			tmp -> next = coda;
			allocati++;
		}
		catch(...) { delete_all(); throw;}
	}
	

	/**
	Implementazione della funzione che restituisce il numero di elementi attuali presenti nel buffer.
	E' un metodo costante in quanto non deve cambiare lo stato delle variabili.
	
	@return allocati numero di elementi attuali presenti nel buffer.
	*/	
	const size_type getAllocati() const { return allocati; }
	
	
	/**
	Implementazione della funzione che restituisce il numero di elementi massimi inseribili nel buffer.
	E' un metodo costante in quanto non deve cambiare lo stato delle variabili.
	
	@return allocati numero di elementi  massimi inseribili nel buffer.
	*/	
	const size_type getSize() const { return size; }
	
	
	/**
	Implementazione della funzione che elimina un elemento in testa al buffer.
	Questa funzione cambia lo stato del buffer qualora sia presente nel buffer almeno un elemeto. 
	*/
	void delete_node()
	{
		if (allocati == 0) { /*no operation*/}
		else
		{
			node *tmp = testa;
			testa = testa -> next;
			delete tmp;
			allocati--;
		}		
	}
	
	
	/**
	Funzione implementata per cancellare completamente il buffers.
	Utilizzata dal distruttore, dal costruttore di copia e dall'overloading dell'operatore di assegnamento.
	*/
	void delete_all()
	{
		while(allocati != 0){delete_node();}
		size = 0; allocati = 0; testa = 0; coda = 0; 
	}
	
	
	class const_iterator; // forward declaration
	
	/**
	Classe iteratore, di tipo forward.
	*/
	class iterator {
		
	private:
		node *n;					///< Puntatore a nodo.

		friend class cbuffer;
		friend class const_iterator;

		
		/** 
		Costruttore privato di inizializzazione usato dalla classe container di solito usato per begin e end.
		
		@param other nodo di inizializzazione .
		*/ 
		iterator(node *other) : n(other) {}
		
		
	public:
		/**
		Costruttore di default dell'iteratore.
		*/
		iterator() : n(0) {}
		
		
		/**
		Costruttore di Copia dell'iteratore.
		
		@param other iteratore da copiare.
		*/
		iterator(const iterator &other): n(other.n){}
				
				
		/**
		Overloading dell'operatore Asssegnamento.
		
		@param other iteratore da copiare.
		@return this iteratore copiato.
		*/
		iterator& operator=(const iterator &other) {n = other.n; return *this; }

		
		/**
		Distruttore.
		*/
		~iterator() {}

		
		/**
		Operazione di dereferenziamento.
		
		@return elem dato riferito dall'iteratore.
		*/
	    const T& operator*() const { return n -> elem; }

		
		/**
		Operazione di dereferenziamento.
		
		@return elem puntatore al dato riferito dall'iteratore.
		*/
		const T* operator->() const { return &(n -> elem); }


		/**
		Operatore di iterazione post-incremento.
		
		@return tmp iteratore post-incremento.
		*/
		iterator operator++(int) 
		{	
				iterator tmp(*this);
				n = n -> next;
				return tmp;
		}

		
		/**
		Operatore di iterazione pre-incremento.
		
		@return this iteratore pre-incremento.
		*/
		iterator& operator++() { n = n -> next; return *this; }

		
		/**
		Operatore di uguaglianza.
		
		@param other altro iteratore da confrontare.
		@return confronto tra i due iteratori.
		*/
		bool operator==(const iterator &other) const { return n == other.n; }

		
		/**
		Operatore di diversità.
		
		@param other altro iteratore da confrontare.
		@return confronto tra i due iteratori.
		*/
		bool operator!=(const iterator &other) const { return n != other.n; }
		
		
		/**
		Operatore di uguaglianza di un const_iterator.
		
		@param other altro iteratore da confrontare.
		@return confronto tra i due iteratori.
		*/
		bool operator==(const const_iterator &other) const { return n == other.n; }

		
		/**
		Operatore di diversità di un const_iterator.
		
		@param other altro iteratore da confrontare.
		@return confronto tra i due iteratori.
		*/
		bool operator!=(const const_iterator &other) const { return n != other.n; }		
	}; // classe iterator
	
	
	/**
	Iterator begin.
	
	@return iterator_begin iteratore all'inizio della sequenza dati.
	*/
	iterator begin() { return iterator(testa); }
	
	
	/**
	Iterator end.
	
	@return iterator iteratore alla fine della sequenza dati.
	*/
	iterator end() { return iterator(0); }
	
	
	/**
	Classe iteratore costante, di tipo forward.
	*/
	class const_iterator {
		
	private:
		const node *n;				///< Puntatore a nodo.

		friend class cbuffer;
		friend class iterator;

		
		/** 
		Costruttore privato di inizializzazione usato dalla classe container
		di solito usato per begin e end.
		
		@param other nodo di inizializzazione .
		*/ 
		const_iterator(const node *other) : n(other){}
	
	
		public:
		/**
		Costruttore di default dell'iteratore.
		*/
		const_iterator() : n(0){}
		
		
		/**
		Costruttore di Copia dell'iteratore.
		
		@param other iteratore da copiare.
		*/
		const_iterator(const const_iterator &other) : n(other.n){}

		
		/**
		Overloading dell'operatore Asssegnamento del const_iterator.
		
		@param other iteratore da copiare.
		@return this iteratore copiato.
		*/
		const_iterator& operator=(const const_iterator &other) { n = other.n; return *this; }

		
		/**
		Distruttore del const_iterator.
		*/
		~const_iterator() {}

		
		/**
		Operazione di dereferenziamento del const_iterator.
		
		@return elem dato riferito dall'iteratore.
		*/
		const T& operator*() const { return n->elem; }

		
		/**
		Operazione di dereferenziamento del const_iterator.
		
		@return elem puntatore al dato riferito dall'iteratore.
		*/
		const T* operator->() const { return &( n-> elem); }
	
		
		/**
		Operatore di iterazione post-incremento del const_iterator.
		
		@return tmp iteratore post-incremento.
		*/
		const_iterator operator++(int)
		{
			const_iterator tmp(*this);
			n = n -> next;
			return tmp;
		}

		
		/**
		Operatore di iterazione pre-incremento del const_iterator.
		
		@return this iteratore pre-incremento.
		*/
		const_iterator& operator++() { n = n -> next; return *this; }

		
		/**
		Operatore di uguaglianza del const_iterator.
		
		@param other altro iteratore da confrontare.
		@return confronto tra i due iteratori.
		*/
		bool operator==(const const_iterator &other) const { return n == other.n; }
		
		
		/**
		Operatore di diversità del const_iterator.
		
		@param other altro iteratore da confrontare.
		@return confronto tra i due iteratori.
		*/
		bool operator!=(const const_iterator &other) const { return n != other.n; }
		
		
		/**
		Operatore di uguaglianza di un const_iterator .
		
		@param other altro iteratore da confrontare.
		@return confronto tra i due iteratori.
		*/
		bool operator==(const iterator &other) const { return n == other.n; }

		
		/**
		Operatore di diversità di un const_iterator.
		
		@param other altro iteratore da confrontare.
		@return confronto tra i due iteratori.
		*/
		bool operator!=(const iterator &other) const { return n != other.n; }
	}; // classe const_iterator
	
	
	/**
	Const Iterator begin.
	
	@return const_iterator_begin iteratore all'inizio della sequenza dati.
	*/
	const_iterator begin() const { return const_iterator(testa); }
	
	
	/**
	Const Iterator End.
	
	@return const_iterator_end iteratore alla fine della sequenza dati.
	*/
	const_iterator end() const { return const_iterator(0); }
	
}; //class cbuffer


/**
	Overloading dell'operatore di "stampa"
	
	@param os output stream
	@param cb cbuffer<T> da cui leggere e stampare i dati
	@return reference all' output stream
*/
template<typename T>
std::ostream& operator<<(std::ostream &os, const cbuffer<T> &cb) 
{
	for(typename cbuffer<T>::size_type i = 0; i < cb.getAllocati(); ++i) os << "<" << cb[i] << "> ";
	return os;
}	


#endif
