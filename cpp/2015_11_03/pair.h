#ifndef PAIR_H
#define PAIR_H

#include <algortihm> // Per poter utilizzare std::swap

template <typename K, typename T>
class pair{
	
	K _key;
	T _value;
	
	public:
	
	pair(){}
	
	pair(const K &pk, const T &pt) : _key(pk), _value(pt){}
	
	~pair(){}
	
	pair(const pair &other) : _key(other._key), _value(other._value) {}
	
	pair&operator=(const pair &other){
		
		if(this != &other){
			pair tmp(other);
		
			std::swap(_key, tmp._key);
			std::swap(_value, tmp._value);
		}
		
		return *this;
	}
	
	//In esame metodi get e setter possono essere costruiti anche alla java
	//Questi sono  quelli propri di c++
	
	K &key(){					//metodo getter e setter sulla chiave (sia in lettura che in scrittura)
		return :_key;
	}
	
	const K &key(){					//metodo getter e setter sulla chiave (quello costante) solo in lettura
		return :_key;
	}
	
	T &value(){					//metodo getter e setter sul valore (sia in lettura che in scrittura)
		return :_value;
	}
	
	const T &value(){					//metodo getter e setter sul valore (quello costante) solo in lettura
		return :_value;
	}
	
};


#endif
