/* Alessio Riccardo Villa
 * Matr. 780748
 *
 * Programmazione ed Amministrazione di Sistema
 * Anno MMXV/MMXVI
 * Progetto d'esame del 15/02/2016
 *
 * cbuffer.h
 * v 2.1.1
 * */

#ifndef CBUFFER_H
#define CBUFFER_H

#include <cassert>
#include <algorithm>
#include <iostream>
#include <iterator>

using namespace std;

template <typename T>
class cbuffer {

	public:

	typedef unsigned int size_type;
	
	private:
	
		T *ptr; ///< Array containing objects of T type
		size_type head; ///< index to the head of the cbuffer
		int tail; ///< number of T objects contained - 1
		size_type size; ///< size of the cbuffer

		void swapB(cbuffer &other) {
			swap(ptr, other.ptr);
			swap(size, other.size);
			swap(head, other.head);
			swap(tail, other.tail);
	}
	
	public:
	
	/**
		Default constructor
	**/
	cbuffer(void) : head(0), tail(-1), size(0), ptr(0) {	}

	/**
		Secondary constructor which allocates an array  with 's' elements
		
		@param s size of cbuffer
	**/
	explicit cbuffer(size_type s) : head(0), tail(0), size(0), ptr(0) {
		ptr = new T[s];
		size = s;
		head = 0;
		tail = -1;
	}

	/**
		Secondary constructor which allocates the cbuffer with size 's';
		it initializes all the elements to 't'
		
		@param s array's dimension
		@param t initialization value 		
	**/
	cbuffer(size_type s, const T &t) : head(0), tail(0), size(0), ptr(0) {
		ptr = new T[s];
		try {
			for (size_type i = 0; i < s; i++)
				ptr[i] = t;
		} catch(...){
			delete[] ptr;
			throw;
		}
		size = s;
		head = 0;
		tail = s - 1;
	}
	/**
		Deconstructor
	**/
	~cbuffer(void){
			delete[] ptr;
			head = 0;
			tail = 0;
			size = 0;
		}
	/**
		Copy costructor
		
		@param b cbuffer which has to be copied
	**/
	cbuffer(const cbuffer& b) : head(0), tail(0), size(0), ptr(0) {
		ptr = new T[b.size];
		try{
			for(size_type i = 0; i < b.size;i++)
				ptr[i] = b.ptr[i];
		} catch(...){
			delete[] ptr;
			throw;
		}
		size = b.size;
		head = b.head;
		tail = b.tail;
	}

	/**
		Assignment operator
		
		@param other cbuffer which has to be copied
	**/
	cbuffer& operator=(const cbuffer &other) {

		if (this != &other) {
			cbuffer tmp(other);
			swapB(tmp);
		}
		return *this;
	}

	/**
		Assignment operator
		
		@param other cbuffer which has to be copied
	**/
	template <typename Q>
	cbuffer& operator=(const cbuffer<Q> &other){
				
		cbuffer<Q> tmp_other(other);
		cbuffer<T> tmp(tmp_other.sizeCbuffer());
		
		for(size_type i = 0; i < tmp_other.contained(); i++)
			tmp.add(static_cast<T>(tmp_other[i]));
		
		swapB(tmp);
		return *this;				
	}

	/**
		Square brackets operator to read/write the cbuffer
		
		@param i index of the required element
		@return constant reference to the required element
	**/
	T& operator[](size_type i) {
		assert(i < this -> contained());
		if (head + i < size)
                    return ptr[head + i];
		else
                    return ptr[i - (size - head - 1) - 1];
		}

	/**
		Read only square brackets operator
		
		@param i index of the required element
		@return constant reference to the required element
	**/
	const T& operator[](size_type i) const {
		assert(i < this -> contained());
		if (head + i < size)
                    return ptr[head + i];
		else
                    return ptr[i - (size - head - 1) - 1];
	}

	/**
		Returns the cbuffer size
		
		@return size value
	**/
	size_type sizeCbuffer(void) const {
		return this -> size;
	}

	/**
		Returns the number of elements contained in cbuffer

		@return number of cbuffer elements
	**/
	size_type contained(void) const {
            return tail + 1;
	}
		
	/**
		Function which adds an element at the end of the cbuffer,
		if the cbuffer is replete than the oldest item will be replaced
		
		@param other element to add in the cbuffer
	**/
	void add(const T &other){
        if (tail > - 1) 
			assert(tail < size);
        if (tail == - 1){ // empty
			ptr[head] = other;
            tail++;
		} else if (tail + 1 < size){
			tail++;
			if(head + tail < size) 
				ptr[tail] = other;
			else
				ptr[tail - (size - head - 1) - 1] = other;
		} else if(tail + 1 == size){
			ptr[head] = other;
			if (head + 1 == size) 
				head = 0;
			else
				head++;
		}
	}

	/**
		Function which removes the oldest element of cbuffer
	**/
	void remove(void){
		if(tail > -1){
			head++;
			tail--;
			if (tail == -1) head = 0;
		}
	}
	
	/**
		Function which removes all the element of cbuffer
	**/
	void remove_all(void){
		if (tail > -1)
			while(tail > -1) remove();
	}
	
	
	class const_iterator;
	class iterator;
	
	/**
		Forward iterator: allows to iterate on cbuffer for R/W operations.
	**/
	class iterator{
		
		friend class cbuffer;
		friend class const_iterator;
		
		T *it; ///< pointer to cbuffer<T> data
		T *end; ///< pointer to the ptr[size - 1] of cbuffer<T>
		bool flag; ///< true if cbuffer<T> is replete, false otherwise
        size_type counter; ///< count the number of iterations
		size_type size; ///< size of cbuffer<T>
		/**
			Private constructor for ptr initialization;
			cbuffer<T> can call it thanks the friend statement.
			
			@param b pointer to cbuffer<T>
			@param e pointer to the ptr[size - 1] of cbuffer<T>
			@param boola true if cbuffer<T> is replete, false otherwise
			@param c count the number of iterations
			@param s size of cbuffer<T>
		**/
		iterator(T* b, T* e, bool boola, size_type c, size_type s) : it(b), end(e), flag(boola), counter(c), size(s) {}
		
	public:
		
		/**
			Default Constructor
		*/
		iterator() : it(0), end(0), flag(0), counter(0), size(0) {}

		/**
			Copy constructor
			
			@param other iterator which has to be copied
		**/
		iterator(const iterator &other) : it(other.it), end(other.end), flag(other.flag), counter(other.counter), size(other.size) {}

		/**
			Assignment operator
			
			@param other iterator which has to be copied
		**/
		iterator& operator=(const iterator &other) {
			it = other.it;
            end = other.end;
			flag = other.flag;
			counter = other.counter;
			size = other.size;
			return *this;
		}

		/**
			Deconstructor
		**/
		~iterator() {}	

		/**
			Dereference 
			@return reference to the pointed data
		*/
		T& operator*() const {
			return *it;
		}
		
		/**
			Pointer
			
			@return pointer to data 
		**/
		T* operator->() const {
			return it;
		}
		
		/**
			Comparison between iterators
			
			@param other iterator which will be compared
			@return true if *this points to other
		**/
		bool operator==(const iterator &other) const {
            if(flag)
				return (this -> counter == other.counter);
            return (other == *this);
        }
		
		/**
			Comparison between iterators (one of them const)
			
			@param other iterator which will be compared
			@return true if *this points to other
		**/
		bool operator==(const const_iterator &other) const {
			if(flag)
				return (this -> counter == other.counter);
            return (other == *this);
			//return (it == other.it);
		}
		
		/**
			Comparison between iterators
			
			@param other iterator which will be compared
			@return true if *this do not points to other
		**/
		bool operator!=(const iterator &other) const {
            if(flag)
				return (this -> counter != other.counter);
            return !(other == *this);
		}
		
		/**
			Comparison between iterators (one of them const)
			
			@param other iterator which will be compared
			@return true if *this do not points to other
		**/
		bool operator!=(const const_iterator &other) const {
			if(flag)
				return (this -> counter != other.counter);
            return !(other == *this);
        }
		
		/**
			Iteration (prefix)
			
			@return iterator in the new position
		**/
		iterator& operator++() {
			if (it == end) 
				it = it - (size - 1);
			else
				it++;
            counter++;
			return *this;
		}
		
		/**
			Iteration (suffix)
			
			@return iterator in the former position
		**/
		iterator operator++(int) {
			iterator tmp(it, end, flag, counter, size);
			if (it == end) 
				it = it - (size - 1);
			else
				it++;
            counter++;
			return tmp;
			
		}
	}; // eof class iterator

	class const_iterator {
		
		friend class cbuffer;
		friend class iterator;
		
		const T *it; ///< pointer to cbuffer<T> data
		const T *end; ///< pointer to the ptr[size - 1] of cbuffer<T>
		bool flag; ///< true if cbuffer<T> is replete, false otherwise
        size_type counter; ///< count the number of iterations
		size_type size; ///< size of cbuffer<T>
		/**
			Private constructor for ptr initialization;
			cbuffer<T> can call it thanks the friend statement.
			
			@param b pointer to cbuffer<T>
			@param e pointer to the ptr[size - 1] of cbuffer<T>
			@param boola true if cbuffer<T> is replete, false otherwise
			@param c count the number of iterations
			@param s size of cbuffer<T>
		**/
		const_iterator(const T* b, const T* e, bool boola, size_type c, size_type s) : it(b), end(e), flag(boola), counter(c), size(s) {}
		
	public:
		
		/**
			Default Constructor
		*/
		const_iterator() : it(0), end(0), flag(0), counter(0), size(0) {}

		/**
			Copy constructor
			
			@param other iterator which has to be copied
		**/
		const_iterator(const const_iterator &other) : it(other.it), end(other.end), flag(other.flag), counter(other.counter), size(other.size) {}

		/**
			Assignment operator
			
			@param other iterator which has to be copied
		**/
		const_iterator& operator=(const const_iterator &other) {
			it = other.it;
            end = other.end;
			flag = other.flag;
			counter = other.counter;
			size = other.size;
			return *this;
		}
		
		/**
			Deconstructor
		**/
		~const_iterator() {}	

		/**
			Dereference 
			@return reference to constant data
		*/
		const T& operator*() const {
			return *it;
		}
		
		/**
			Pointer
			
			@return pointer to constant data 
		**/
		const T* operator->() const {
			return it;
		}
		
		/**
			Comparison between constant iterators
			
			@param other const_iterator which will be compared
			@return true if *this points to other
		**/
		bool operator==(const const_iterator &other) const {
            if(flag)
				return (this -> counter == other.counter);
            return (other == *this);}
		
		/**
			Comparison between iterators (only one of them const)
			
			@param other iterator which will be compared
			@return true if *this points to other
		**/
		bool operator==(const iterator &other) const {
			if(flag)
				return (this -> counter == other.counter);
            return (other == *this);
		}
		
		/**
			Comparison between iterators
			
			@param other iterator which will be compared
			@return true if *this do not points to other
		**/
		bool operator!=(const const_iterator &other) const {
            if(flag)
				return (this -> counter != other.counter);
            return !(other == *this);
		}
		
		/**
			Comparison between iterators (only one of them const)
			
			@param other iterator which will be compared
			@return true if *this do not points to other
		**/
		bool operator!=(const iterator &other) const {
			if(flag)
				return (this -> counter != other.counter);
            return !(other == *this);
        }
		
		/**
			Iteration (prefix)
			
			@return iterator in the new position
		**/
		const_iterator& operator++() {
			if (it == end) 
				it = it - (size - 1);
			else
				it++;
            counter++;
			return *this;
		}
		
		/**
			Iteration (suffix)
			
			@return iterator in the former position
		**/
		const_iterator operator++(int) {
			const_iterator tmp(it, end, flag, counter, size);
			if (it == end) 
				it = it - (size - 1);
			else
				it++;
            counter++;
			return tmp;
			
		}
	}; // eof const_iterator
        
	/**
		Claim iterator
		
		@return R/W iterator to the begin of the data sequence
	*/
	iterator begin() {
            return iterator(ptr + head, ptr + size - 1, (tail == size - 1), 0, size);
	}

	/**
		Claim iterator
		
		@return R/W iterator at the end of data sequence
	*/
	iterator end() {
        size_type i = 0;
        if (head + tail < size)
			i = tail + 1;
        else
			i = (tail - (size - head - 1));
		
        return iterator(ptr + i, ptr + size - 1, (tail == size - 1), size, size);
    }

	/**
		Claim const_iterator
		
		@return R/W iterator to the begin of the data sequence
	*/
	const_iterator begin() const {
		return const_iterator(ptr + head, ptr + size - 1, (tail == size - 1), 0, size);
	}

	/**
		Claim const_iterator
		
		@return R/W iterator at the end of data sequence
	*/
	const_iterator end() const {
	    size_type i = 0;
        if (head + tail < size)
			i = tail + 1;
        else
			i = (tail - (size - head - 1));
		
        return const_iterator(ptr + i, ptr + size - 1, (tail == size - 1), size, size);
    }
	
	
	
}; // eof class cbuffer<T>

/**
	Stream operator redefined to output the contents of cbuffer
	
	@param os output stream
	@param cbuffer<T> cbuffer to output
	@return reference to the output stream
**/
template<typename T> ostream &operator<<(ostream &os, const cbuffer<T> &cbu) {
	for(typename cbuffer<T>::size_type i = 0; i < cbu.contained(); i++){
		os << cbu[i] << " ";
	} return os;
}


/**
	Stream operator redefined to output the strings contenained in cbuffer
	
	@param os output stream
	@param cbuffer<string> cbuffer to output
	@return reference to the output stream
**/
ostream &operator<<(ostream &os, const cbuffer<string> &cbu) {
	for(typename cbuffer<string>::size_type i = 0; i < cbu.contained(); i++){
		os << cbu[i] << endl;
	} return os;
}

/**
	The function check if an unary predicate is true or false 
	with respect to each element of the cbuffer in input.
	
	@param cb cbuffer which elements will be given to the unary predicate
	@param p unary predicate which has to be checked
**/
typedef unsigned int size_type; // !
	template <typename T, typename Q>
	void check(const cbuffer<T>& cb, const Q &p)
	{
		for(size_type i = 0; i < cb.contained(); i++)
		{
				cout << boolalpha << "[" << i << "]: " << p(cb[i]) << " ";
		} cout << endl;
	};

#endif
// eof cbuffer.h
