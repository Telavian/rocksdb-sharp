﻿using System;

namespace RocksDbSharp
{
    #if ROCKSDB_FILTER_POLICY
    public class BloomFilterPolicy : IDisposable, IRocksDbHandle
    {
        private IntPtr handle;

        public IntPtr Handle { get { return handle; } }

        private BloomFilterPolicy(IntPtr handle)
        {
            this.handle = handle;
        }

        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                Native.rocksdb_filterpolicy_destroy(handle);
                handle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Return a new filter policy that uses a bloom filter with approximately
        /// the specified number of bits per key.
        /// bits_per_key: bits per key in bloom filter. A good value for bits_per_key
        /// is 10, which yields a filter with ~ 1% false positive rate.
        /// use_block_based_builder: use block based filter rather than full fiter.
        /// If you want to builder full filter, it needs to be set to false.
        /// Callers must delete the result after any database that is using the
        /// result has been closed.
        /// Note: if you are using a custom comparator that ignores some parts
        /// of the keys being compared, you must not use NewBloomFilterPolicy()
        /// and must provide your own FilterPolicy that also ignores the
        /// corresponding parts of the keys.  For example, if the comparator
        /// ignores trailing spaces, it would be incorrect to use a
        /// FilterPolicy (like NewBloomFilterPolicy) that does not ignore
        /// trailing spaces in keys.
        /// </summary>
        /// <param name="bits_per_key">Bits per key.</param>
        public static BloomFilterPolicy Create(int bits_per_key = 10, bool use_block_based_builder = true) {
            IntPtr handle = use_block_based_builder ? Native.rocksdb_filterpolicy_create_bloom(bits_per_key) : Native.rocksdb_filterpolicy_create_bloom_full(bits_per_key);
            return new BloomFilterPolicy(handle);
        }
    }
    #endif
}