//*********************************************************************//
//* NB ::::questo codice non è completo!! lo si termina venerdi!! XD  *//
//*********************************************************************//
#ifndef AVECTOR_H
#define AVECTOR_H

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
		
		node() : netx(0){}
		~node() {}					//solo se classe nodo alloca risorse allora il distruttore deve esistere, altrimenti è un distruttore vuoto
		
		node(const node &other) : data(other.data), next(other.next) {}
		node&operator=(const node &other){
			if(this != oter){
				data = other.data;
				next = other.next; 
			}
			
			return *this;
		}
	};
	
	node *_head;
	int _size;
	
	public:
	
	avector() : _head(0), size(0) {}
	
	avector(const avector& other);
	
	avector&operator=(const avector &other)
	
	void add(const K &pk, const T &pt){
		
		if(check(pk)){			//metodo check(da implementare!!) serve per verificare che la chiave da 
								//inserire non sia già stata utilizzata in un altro nodo
			if(_head == 0){
				_head = new(node(pk, pt));

			}
		}else{
			node *tmp = _head;
			_head = new node(pk, pt);
			_head -> next = tmp;
		}
		
		_size++;
	}
	
	bool check(const K &pk) const{
		node *curr = _head;
		compK comp;	//compK funtore per comparare due chiavi
		
		while(curr != 0){
			
			if(comp(curr -> data.key(), pk)){	
				return false;
			}
			curr = curr -> next;
		}
		return true;
	}
	
	
};


#endif
