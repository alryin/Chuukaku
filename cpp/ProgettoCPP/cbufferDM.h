/*
Nome: Davide 
Cognome: Marcon 
Matricola: 781344
*/

#ifndef CBUFFER_H
#define CBUFFER_H
#include <iostream>
#include <cassert> 
#include <algorithm>
#include <iterator>
#include <cstddef>  

template <typename T>
class cbuffer {
	
	public:
		typedef unsigned int size_type;
		
	private:
		T* ptr;
		T* head;
		T* tail;
		T* sentry;
		size_type size;
		size_type entered;
		
		/**
			Auxiliary procedure which assigns pointers and data of one cbuffer<T> object to this
			
			@param other cbuffer from which data are read
		*/
		void swap(cbuffer &other) {
			std::swap(ptr, other.ptr);
			std::swap(head, other.head);
			std::swap(tail, other.tail);
			std::swap(sentry, other.sentry);
			std::swap(size, other.size);
			std::swap(entered, other.entered);
		}
		
	public:
	
		/**
			Default constructor
		*/
		cbuffer(void) : ptr(0), head(0), tail(0), sentry(0), size(0), entered(0) {
			ptr = new T[16];
			size = 16;
			head = ptr;
			tail = ptr;
			sentry = ptr + 1;
		}
		
		/**
			Secondary constructor which allocates the cbuffer at a given capacity
			
			@param size cbuffer dimension
		*/
		explicit cbuffer(size_type size) : ptr(0), head(0), tail(0), sentry(0), size(0), entered(0) {
			ptr = new T[size + 1];
			this -> size = size + 1;
			head = ptr;
			tail = ptr;
			sentry = ptr + 1;
		}
		
		/**
			Secondary constructor which allocates the cbuffer at a given capacity and fills it
			with an initialization value
			
			@param size cbuffer dimension
			@param init initialization value
		*/
		cbuffer(size_type size, const T &init) : ptr(0), head(0), tail(0), sentry(0), size(0), entered(0) {
			ptr = new T[size + 1];
			
			for(size_type i = 0; i < size; ++i) ptr[i] = init;
			
			head = ptr;
			tail = ptr + size - 1;
			sentry = ptr + size;
			this -> size = size + 1;
			entered = size;
		}
		
		/**
			Copy constructor
			
			@param other cbuffer from which data are copied
		*/
		cbuffer(const cbuffer &other) : ptr(0), head(0), tail(0), sentry(0), size(0), entered(0) {
			ptr = new T[other.size];
			
			for(size_type i = 0; i < other.entered; ++i) ptr[i] = other.ptr[i];
			
			head = ptr + (other.head - other.ptr);
			tail = ptr + (other.tail - other.ptr);
			sentry = ptr + (other.sentry - other.ptr);
			size = other.size;
			entered = other.entered;
		}
		
		/**
			Deconstructor
		*/
		~cbuffer(void) {
			delete[] ptr;
			ptr = 0;
			head = 0;
			tail = 0;
			sentry = 0;
			size = 0;
			entered = 0;
		}
		
		/**
			Assignment operator
			
			@param other cbuffer from which data are copied
		*/
		cbuffer& operator=(const cbuffer &other) {
			if(this != &other) {
				cbuffer tmp(other);
				swap(tmp);
			}
			return *this;
		}
		
		/**
			Assignment operator
			
			@param other cbuffer from which data are copied
		*/
		template <typename Q>
		cbuffer& operator=(const cbuffer<Q> &other) {
			cbuffer<T> tmp(other.get_size());			
			for(size_type i = 0; i < other.get_entered(); ++i) tmp.add(static_cast<T>(other[i]));
			
			swap(tmp);
			return *this;
		}
		
		/**
			Getter which returns the cbuffer size
			
			@return cbuffer dimension
		*/
		size_type get_size(void) const {
			return size - 1;
		}
		
		/**
			Getter which returns the number of items contained in the cbuffer
			
			@return cbuffer number of items contained in the cbuffer
		*/
		size_type get_entered(void) const {
			return entered;
		}
		
		/**
			Getter which returns a reference to the tail of the cbuffer
			
			@return reference to the tail of the cbuffer<T>
		*/
		const T& get_tail(void) const {
			assert(entered > 0 && tail != 0);
			
			return *tail;	
		}
		
		/**
			Procedure which adds a new element to the cbuffer
			
			@param item element to insert into the cbuffer
		*/
		void add(const T& item) {
			assert(head != 0 && tail != 0 && sentry != 0);
			
			if(entered > 0) {
				tail = ptr + ((tail - ptr) + 1) % size;
				*tail = item;
				sentry = ptr + ((sentry - ptr) + 1) % size;
				if(sentry == head) head = ptr + ((head - ptr) + 1) % size;
				if(entered < size - 1) ++entered;
			} else {
				*tail = item;
				++entered;
			}	
		}
		
		/**
			Procedure which removes the element at the head of the cbuffer
		*/
		void remove(void) {
			assert(head != 0 && tail != 0 && sentry != 0);
			if(entered > 0) {
				head = ptr + ((head - ptr) + 1) % size;
				--entered;
				if(entered == 0) {
					head = ptr;
					tail = ptr;
					sentry = ptr + 1;
				}
			}	
		}
		
		/**
			Procedure which removes all elements in the buffer
		*/
		void flush(void) {
			assert(head != 0 && tail != 0);
			if(entered > 0) {
				size_type m = entered;
				for(size_type i = 0; i < m; ++i) remove();
			}
		}
		
		/**
			Function which returns the element in the cbuffer at the given index
			
			@param index index of the element to return
			@return constant reference to the element at the given index
		*/
		const T& get_item(size_type index) const {
			assert(index < entered && head != 0);
			
			size_type realIndex = (head - ptr) + index;
			if(realIndex >= size) return ptr[realIndex - size];
			else return ptr[realIndex];
		}

		/**
			Procedure which modifies the element in the cbuffer at the given index
			
			@param index index of the element to modify
			@param value value to insert into the cbuffer at the given index;
		*/
		void set_item(size_type index, const T& value) {
			assert(index < entered && head != 0);
			
			size_type realIndex = (head - ptr) + index;
			if(realIndex >= size) ptr[realIndex - size] = value;
			else ptr[realIndex] = value;	
		}
		
		/**
			Square brackets operator to read/write the cbuffer at the given index
			
			@param index index of the element to get/set
			@return reference to the element at the given index
		*/
		T& operator[](size_type index) {
			assert(index < entered && head != 0);

			size_type realIndex = (head - ptr) + index;
			if(realIndex >= size) return ptr[realIndex - size];
			else return ptr[realIndex];
		}
		
		/**
			Square brackets read-only operator 
			
			@param index of the element to get
			@return constant reference to the element at the given index
		*/
		const T& operator[](size_type index) const {
			assert(index < entered && head != 0);

			size_type realIndex = (head - ptr) + index;
			if(realIndex >= size) return ptr[realIndex - size];
			else return ptr[realIndex];
		}
		
		class iterator;
		class const_iterator;

		/**
			Iterator to read/write data of cbuffer<T>
		*/
		class iterator {
		
			friend class cbuffer; 
			friend class const_iterator; 
		
			T* ptr;
			T* first_cell; // array starting point
			size_type entered;
			size_type size;

			/**
				Private constructor to initialize pointers and data
				
				@param p data pointer of cbuffer<T>
				@param start pointer to the first cell in memory of cbuffer<T>
				@param n number of entered elements in the cbuffer<T>
				@param sz size of cbuffer<T>
			*/
			iterator(T* p, T* start, size_type n, size_type sz) : ptr(p), first_cell(start), entered(n), size(sz) {}

			public:
			
				typedef std::forward_iterator_tag iterator_category;
				typedef T value_type;
				typedef ptrdiff_t difference_type;
				typedef T* pointer;
				typedef T& reference;

				/**
					Default constructor
				*/
				iterator() : ptr(0), first_cell(0), entered(0), size(0) {}

				/**
					Copy constructor
				*/
				iterator(const iterator &other) : ptr(other.ptr), first_cell(other.first_cell), entered(other.entered), size(other.size) {}

				/**
					Deconstructor
				*/
				~iterator() {}
				
				/**
					Assignment operator
				*/
				iterator& operator=(const iterator &other) {
					ptr = other.ptr;
					first_cell = other.first_cell;
					entered = other.entered;
					size = other.size;
					return *this;		
				}
	
				/**
					Dereference operator
					
					@return reference to pointed value
				*/
				T& operator*() const {
					assert(entered > 0);
					
					return *ptr;
				}

				/**
					Pointer operator
					
					@return pointer to the value 
				*/
				T* operator->() const {
					return ptr;
				}

				/**
					Comparison iterator/iterator
					
					@param other iterator to compare
					@return true if *this points to the same data as other
				*/
				bool operator==(const iterator &other) const {
					return (ptr == other.ptr);	
				}

				/**
					Comparison iterator/iterator
					
					@param other iterator to compare
					@return true if *this does not point to the same data as other
				*/
				bool operator!=(const iterator &other) const {
					return !(other == *this);		
				}

				/**
					Comparison iterator/const_iterator
					
					@param other const_iterator to compare
					@return true if *this points to the same data as other
				*/
				bool operator==(const const_iterator &other) const {
					return (ptr == other.ptr);	
				}

				/**
					Comparison iterator/const_iterator
					
					@param other iterator to compare
					@return true if *this does not point to the same data as other
				*/
				bool operator!=(const const_iterator &other) const {
					return !(other == *this);	
				}
				
				/**
					Repositioning (prefixed)
					
					@return iterator at the new position
				*/
				iterator& operator++() {
					size_type i = (ptr - first_cell) + 1;
					if(i == size) ptr = first_cell;
					else ++ptr;
					
					return *this;
				}
				
				/**
					Repositioning (postfixed)
					
					@return iterator at the old position
				*/
				iterator& operator++(int) {
					iterator tmp(ptr);
					
					size_type i = (ptr - first_cell) + 1;
					if(i == size) ptr = first_cell;
					else ++ptr;
					
					return tmp;
				}	
		};

		/**
			Const iterator to read data of cbuffer<T>
		*/
		class const_iterator {
			
			friend class cbuffer; 
			friend class iterator; 
			
			const T* ptr;
			const T* first_cell; // array starting point
			size_type entered;
			size_type size;

			/**
				Private constructor to initialize pointers and data
				
				@param p data pointer of cbuffer<T>
				@param start pointer to the first cell in memory of cbuffer<T>
				@param n number of entered elements in the cbuffer<T>
				@param sz size of cbuffer<T>
			*/
			const_iterator(const T* p, const T* start, size_type n, size_type sz) : ptr(p), first_cell(start), entered(n), size(sz) {}

			public:
			
				typedef std::forward_iterator_tag iterator_category;
				typedef T value_type;
				typedef ptrdiff_t difference_type;
				typedef const T* pointer;
				typedef const T& reference;
				
				/**
					Default constructor
				*/
				const_iterator () : ptr(0), first_cell(0), entered(0), size(0) {}

				/**
					Copy constructor
				*/
				const_iterator (const const_iterator  &other) : ptr(other.ptr), first_cell(other.first_cell), entered(other.entered), size(other.size) {}
				
				/**
					Deconstructor
				*/
				~const_iterator () {}

				/**
					Assignment operator
				*/
				const_iterator & operator=(const const_iterator  &other) {
					ptr = other.ptr;
					first_cell = other.first_cell;
					entered = other.entered;
					size = other.size;
					return *this;		
				}

				/**
					Dereference operator
					
					@return reference to const pointed value
				*/
				const T& operator*() const {
					return *ptr;		
				}
			
				/**
					Pointer operator
					
					@return pointer to the const value
				*/
				const T* operator->() const {
					return ptr;
				}

				/**
					Comparison const_iterator/const_iterator
					
					@param other const_iterator to compare
					@return true if *this points to the same data as other
				*/
				bool operator==(const const_iterator  &other) const {
					return (ptr == other.ptr);	
				}

				/**
					Comparison const_iterator/const_iterator
					
					@param other const_iterator to compare
					@return true if *this does not point to the same data as other
				*/
				bool operator!=(const const_iterator  &other) const {
					return !(other == *this);		
				}

				/**
					Comparison const_iterator/iterator
					
					@param other iterator to compare
					@return true if *this points the same data as other
				*/
				bool operator==(const iterator  &other) const {
					return (ptr == other.ptr);	
				}

				/**
					Comparison const_iterator/iterator
					
					@param other iterator to compare
					@return true if *this does not point to the same data as other
				*/
				bool operator!=(const iterator  &other) const {
					return !(other == *this);		
				}
				
				/**
					Repositioning (prefixed)
					
					@return iterator at the new position
				*/
				const_iterator& operator++() {
					size_type i = (ptr - first_cell) + 1;
					if(i == size) ptr = first_cell;
					else ++ptr;
					
					return *this;
				}
				
				/**
					Repositioning (postfixed)
					
					@return iterator at the old position
				*/
				const_iterator& operator++(int) {
					const_iterator tmp(ptr);
					
					size_type i = (ptr - first_cell) + 1;
					if(i == size) ptr = first_cell;
					else ++ptr;
					
					return tmp;
				}	
		};

		/**
			Iterator request
			
			@return read/write iterator starting at the beginning of the data sequence
		*/
		iterator begin() {
			return iterator(head, ptr, entered, size);
		}

		/**
			Iterator request
			
			@return read/write iterator starting at the end of the data sequence
		*/
		iterator end() {
			if(entered == 0) return iterator(tail, 0, 0, 0);
			return iterator(sentry, 0, 0, 0);
		}

		/**
			Request of a const_iterator
			
			@return read-only iterator starting at the beginning of the data sequence
		*/
		const_iterator begin() const {
			return const_iterator(head, ptr, entered, size);
		}

		/**
			Request of a const_iterator
			
			@return read-only iterator starting at the end of the data sequence
		*/
		const_iterator end() const {
			if(entered == 0) return const_iterator(tail, 0, 0, 0);
			return const_iterator(sentry, 0, 0, 0);
		}		
};

/**
	Insertion operator
	
	@param os output stream
	@param cb cbuffer<T> from which read and print data
	@return reference to the output stream
*/
template<typename T>
std::ostream& operator<<(std::ostream &os, const cbuffer<T> &cb) {
	for(typename cbuffer<T>::size_type i = 0; i < cb.get_entered(); ++i) os << cb[i] << " ";
	return os;
}		

#endif
