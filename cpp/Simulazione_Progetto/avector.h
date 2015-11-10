#ifndef AVECTOR_H
#define AVECTOR_H

#include <algorithm>
#include <stdexcept>	//per std::runtime_error 
#include "pair.h"
#include <iterator> // std::forward_iterator_tag
#include <cstddef>  // std::ptrdiff_t


//classe per definire l'eccezione che stiamo creando 
//class p: public OtherCass  -->ereditarietà
class key_not_found : public std::runtime_error{
	public:
	key_not_found(const char *error) : std::runtime_error(error) {}
};

class duplicated_key : public std::runtime_error{
	public:
	duplicated_key(const char *error) : std::runtime_error(error) {}
};


//classe vettore associativo realizzata con una lista semplice di nodi

template <typename K, typename T, typename compK >	//scelgo di templare sui tipi chiave e valore
													//(assumo che avector.K == pair.K && avector.T == pair.T )
													// Il terzo parametro serve per il confronto tra due chiavi, che non so di vche tipo siano 
class avector{	
	
	struct node {
		pair<K, T> data;
		node *next;
	
		node(const K &pk, const T &pt) : data(pk, pt), next(0) {}
		node(const K &pk, const T &pt, node *nd) : data(pk, pt), next(nd) {}		//next non è costante paerchè nella struttura nodo non è costante
		
		node() : next(0){}
		~node() {}					//solo se classe nodo alloca risorse allora il distruttore deve esistere, altrimenti è un distruttore vuoto
		
		node(const node &other) : data(other.data), next(other.next) {}
		
		node&operator=(const node &other){
			if(this != other){
				data = other.data;
				next = other.next; 
			}
			return *this;
		}
	};
	
	node *_head;
	int _size;
	
	node *find_helper(const K &k) const{
		node *curr = _head;
		compK comp;	//compK funtore per comparare due chiavi
		
		while(curr != 0){
			if(comp(curr -> data.key(), k))	return curr;			
			curr = curr -> next;
		}
		return 0;
	}
	
	
	public:
	
	avector() : _head(0), _size(0) {}
	
	
	~avector() {
		clear();
	}
	
	
	void clear(){
		node *tmp = _head;
		node *tmp2 = 0;
		
		while(tmp != 0){
			tmp2 = tmp -> next; 
			delete tmp;
			tmp = tmp2;
		}
	}
	
	
	avector(const avector& other): _head(0), _size(0){
		node *tmp = other._head;
		
		try{
			while(tmp != 0){
				add(tmp -> data.key(), tmp -> data.value());
				tmp = tmp -> next;
			}
		} catch(...){
			clear();
			throw;	//Rilancio dell'eccezione!!
		}
	}
	
	
	avector&operator=(const avector &other){
		if(this != &other){
			avector tmp(other);
			std::swap(this -> _head, tmp._head);
			std::swap(this -> _size, tmp._size);
		}
		return *this;
	}
	
	
	void add(const K &pk, const T &pt){
		
		if(check(pk)){			//metodo check(da implementare!!) serve per verificare che la chiave da 
								//inserire non sia già stata utilizzata in un altro nodo
			if(_head == 0){
				_head = new node(pk, pt);	//non si gestiscono eccezsioni perchè non si modificano i dati precedenti alla new 
			}
			else{
				node *tmp = _head;
				_head = new node(pk, pt);
				_head -> next = tmp;
			}
		}else {
			throw duplicated_key("Chiave duplicata!!");}
		_size++;
		
		
	}
	
	
	bool check(const K &pk) const{
		node *tmp = find_helper(pk);
		return (tmp == 0);
	}

	
	T &find(const K &k) {
		node *n = find_helper(k);
		
		if(n != 0)	return n -> data.value();
		
		throw key_not_found("key not found, insert a valid key!!");	//key_not_found() tipo di dato
	}
	
	
	const T &find(const K &k) const {
		node *n = find_helper(k);
		
		if(n != 0){return n -> data.value(); }
		
		throw key_not_found("key not found, insert a valid key!!");
	}
	
	
	template <typename iteratorK, typename iteratorT>
	void fill(iteratorK bk, iteratorK ek, iteratorT bt, iteratorT et){
		avector tmp;
		
		for( ; bk != ek ; ++bk, ++bt){
			tmp.add( *bk, *bt);
		}
		
		std::swap(this -> _head, tmp._head);
		std::swap(this -> _size, tmp._size);
	}

	
	class const_iterator {
		//	
	public:
		typedef std::forward_iterator_tag iterator_category;
		typedef pair<K, T>               		 value_type;
		typedef ptrdiff_t               		 difference_type;
		typedef const pair<K, T>*                pointer;
		typedef const pair<K, T>&                reference;

	
		const_iterator() {
			n = 0;
		}
		
		const_iterator(const const_iterator &other) {
			n = other.n;
		}

		const_iterator& operator=(const const_iterator &other) {
			n = other.n;
			return *this;
		}

		~const_iterator() {}

		// Ritorna il dato riferito dall'iteratore (dereferenziamento)
		const reference operator*() const {
			return n -> data;
		}

		// Ritorna il puntatore al dato riferito dall'iteratore
		const pointer operator->() const {
			return &(n -> data;
		}
		
		// Operatore di iterazione post-incremento
		const_iterator operator++(int) {
			const_iterator tmp(*this);
			n = n-> next;
			return tmp;
		}

		// Operatore di iterazione pre-incremento
		const_iterator& operator++() {
			n = n -> next;
			return *this;
		}

		// Uguaglianza
		bool operator==(const const_iterator &other) const {
			return n == other.n;
		}
		
		// Diversita'
		bool operator!=(const const_iterator &other) const {
			return n != other.n;
		}

	private:
		node *n;		//Dati membro

		// La classe container deve essere messa friend dell'iteratore per poter
		// usare il costruttore di inizializzazione.
		friend class avector;

		// Costruttore privato di inizializzazione usato dalla classe container
		// tipicamente nei metodi begin e end
		const_iterator(const node *pn) { 
			n = pn;
		}
					
	}; // classe const_iterator
	
	// Ritorna l'iteratore all'inizio della sequenza dati
	const_iterator begin() const {
		return const_iterator(_head);
	}
	
	// Ritorna l'iteratore alla fine della sequenza dati
	const_iterator end() const {
		return const_iterator(0);
	}
	
};

	
#endif
