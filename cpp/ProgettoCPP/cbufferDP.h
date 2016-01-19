#ifndef CBUFFER_H
#define CBUFFER_H

/**
La classe cbuffer rappresenta un buffer circolare templato,
implementato tramite l'uso di una lista concatenata singolarmente.
Una volta decisa la dimensione del buffer, questo non può essere più modificato.

@param size è la dimensione del buffer
**/

template <typename T>
class cbuffer
{
	public:
	typedef unsigned int size_type;
	
	/**
	La struttura node rappresenta un nodo della lista-buffer
	**/
	struct node
	{
		T elem; ///< Dato Templato del nodo 
		node* next; ///< Puntatore al nodo successivo
		
		node(const T &el): elem(el), next(0) {}
		~node(){}	
		/*node&operator=(const node &other)
		{
			
					
		}*/
	};
	
	private:
	size_type	size;			 ///< Dimensione massima del Buffer
	size_type allocati;			///< Allocati è il numero di elementi allocati nel buffer, allocati <= size
	node* begin;				///< Puntatore iniziale al Buffer
	node* current;				///< Puntatore all'elemento corrente del Buffer
	
	public:	
	cbuffer(size_type st) : size(st), allocati(0), begin(0), current(0){}
	
	/*const T cbuffer&operator[](size_type s)
	{
		if(s <= size && s<= allocati)
		{
			
			
			
			for(int i =0; i<s; i++)
			{
				
			}
			
			return tmp;
		}
		
	}
	*/
	/*cbuffer&operator=(const cbuffer &other)
	{
			if(this != &other)
			{
				
			}
			
	}
	*/
	
	void add(const T &other)
	{
		if(other != 0){
			if(begin == 0 || allocati == 0)
			{
				add_helper(begin, other);
				allocati++;
			}
			else if(allocati < size-1)
			{
				add_helper(current, other);
				allocati ++;
			}
			else if(allocati == size-1)
			{
				add_helper(current , other);
				allocati ++;
				current -> next = begin;						
			}
			else
			{	
				begin = begin -> next;
				current = current -> next;
				*(current -> elem) = other; 
			}
		}
	}
	
	void add_helper(node *_node, const T &other)
	{
		node tmp = new node(other);
		if(_node == 0){_node = &tmp;	}	//Nel caso _node sia il primo nodo che inserisco nella lista _node == head
		else{_node -> next = &tmp;		}	//Nel caso _node sia inserito successivamente
		current = &tmp;
	}
	
	
	/**
	Il Metodo delete_node cancella il nodo in testa alla lista.
	**/
/*	void delete_node()
	{
		node *tmp = begin;
		begin = begin -> next;
		
		current -> next = 0;
	/*	if(allocati < size)
		{
			
			
		} 
		else 
		{
			current -> next = 0
		}
		
		allocati --;
		
	}
	*/
};




#endif
