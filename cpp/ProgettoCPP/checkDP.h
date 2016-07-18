#ifndef CHECK_H
#define CHECK_H

#include "cbuffer.h"
#include <iostream>


/**
La funzione check prende in input un qualsiasi buffer cricolare templato 
e un qualsiasi predicato unario, che deve essere compatibile con il tipo
di dato del buffer, e applica il predicato ad ogni singolo elemento del buffer.

@param cb buffer circolare templato
@param P predicato unario che deve essere applicato al buffer circolare
*/
typedef unsigned int size_type; 
	template <typename T, typename compT>
	void check(const cbuffer<T>& cb,const compT &P)
	{
		size_type all = cb.getAllocati();
		size_type index;
		
		for(index = 0; index < all; index++)
		{
			if( P(cb[index])) {std::cout << "[" << index << "]: true" << std::endl; }
			else {std::cout << "[" << index << "]: false" << std::endl; }
		}
	};


#endif

