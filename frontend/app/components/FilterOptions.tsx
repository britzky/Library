'use client';

import React from 'react'
import { Listbox, ListboxButton, ListboxOption, ListboxOptions } from '@headlessui/react';

interface FilterOptionsProps {
    filter: {
        sortBy: string;
        filterBy: string;
    };
    onFilterChange: (filter: FilterOptionsProps['filter']) => void;
}

const FilterOptions: React.FC<FilterOptionsProps> = ({ filter, onFilterChange }) => {
    const filterOptions = ['All', 'Available', 'Unavailable'];
    const sortOptions = ['title', 'author', 'availability'];

  return (
    <div className="space-y-4">
        <h2 className="text-xl font-semibold">Filters</h2>
        <div>
            <label htmlFor="author" className="block text-sm font-medium text-gray-700">
                Filter By
            </label>
            <div className="space-y-2 space-x-7">
            {filterOptions.map((option) => (
                <button
                    key={option}
                    onClick={() => onFilterChange({ ...filter, filterBy: option === 'All' ? '' : option.toLowerCase() })}
                    className={` min-w-[100px] px-3 py-2 rounded-md text-sm font-medium ${
                        filter.filterBy === (option === 'All' ? '' : option.toLowerCase())
                            ? 'bg-blue-500 text-white'
                            : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
                    }`}
                >
                    {option}
                </button>
            ))}
            </div>
        </div>
        <div>
            <label htmlFor="sortBy" className='block text-sm font-medium text-gray-700 mb-2'>
                Sort By
            </label>
            <Listbox value={filter.sortBy} onChange={(value) => onFilterChange({...filter, sortBy: value })}>
                <ListboxButton className="relative w-full py-2 pl-3 pr-10 text-left bg-white rounded-lg shadow-md cursor-default focus:outline-none focus-visible:ring-2 focus-visible:ring-opacity-75 focus-visible:ring-white focus-visible:ring-offset-orange-300 focus-visible:ring-offset-2 focus-visible:border-indigo-500 sm:text-sm">
                    {filter.sortBy ? filter.sortBy.charAt(0).toUpperCase() + filter.sortBy.slice(1) : 'Select sorting'}
                </ListboxButton>
                <ListboxOptions className="absolute w-full py-1 mt-1 overflow-auto text-base bg-white rounded-md shadow-lg max-h-60 ring-1 ring-black ring-opacity-5 focus:outline-none sm:text-sm">
                    {sortOptions.map((option) => (
                        <ListboxOption key={option} value={option.toLowerCase()} className="cursor-default select-none relative py-2 pl-10 pr-4">
                            {({ selected, active }) => (
                            <li
                                className={`${
                                active ? 'text-amber-900 bg-amber-100' : 'text-gray-900'
                                } ${selected ? 'font-medium' : 'font-normal'}`}
                            >
                                <span className="block truncate">{option}</span>
                                {selected && (
                                <span className="absolute inset-y-0 left-0 flex items-center pl-3 text-amber-600">
                                    <svg className="w-5 h-5" viewBox="0 0 20 20" fill="currentColor">
                                    <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                                    </svg>
                                </span>
                                )}
                            </li>
                            )}
                        </ListboxOption>
                    ))}
                </ListboxOptions>
            </Listbox>
        </div>
    </div>
  )
}

export default FilterOptions
