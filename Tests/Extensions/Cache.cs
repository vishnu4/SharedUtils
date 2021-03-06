﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SharedUtils.Tests.Objects;
using static SharedUtils.Extensions.CacheExtensions;
using System.Threading.Tasks;

namespace SharedUtils.Tests.Extensions
{
    [TestClass]
    public class Cache
    {

        [TestInitialize]
        public void Initialize()
        {
            //
        }

        [TestMethod]
        public void GetFromCache()
        {
            using (DictionaryCache cache = new DictionaryCache())
            {
                List<string> testList = new List<string>() { "aaaa", "bbbb", "cccc" };
                const string listStringCacheKey = "alksdjf";
                List<string> firstResult = cache.Get<List<string>>(listStringCacheKey, () => { return testList; });
                CollectionAssert.AreEquivalent(testList, firstResult);

                List<string> secondResult = cache.Get<List<string>>(listStringCacheKey, () => { return new List<string>(); });  //point here is that the old list is cached, doesn't matter what i pass into the function
                CollectionAssert.AreEquivalent(testList, secondResult);

                cache.Remove(listStringCacheKey);
                List<string> thirdResult = cache.Get<List<string>>(listStringCacheKey, () => { return new List<string>(); });
                CollectionAssert.AreNotEquivalent(testList, thirdResult);
            }
        }

        [TestMethod]
        public async Task GetFromCacheAsync()
        {
            using (DictionaryCache cache = new DictionaryCache())
            {
                List<string> testList = new List<string>() { "aaaa", "bbbb", "cccc" };
                const string listStringCacheKey = "alksdjf";
                List<string> firstResult = await cache.GetAsync<List<string>>(listStringCacheKey, () => { return Task.FromResult(testList); });
                CollectionAssert.AreEquivalent(testList, firstResult);

                List<string> secondResult =await cache.GetAsync<List<string>>(listStringCacheKey, () => { return Task.FromResult(new List<string>()); });  //point here is that the old list is cached, doesn't matter what i pass into the function
                CollectionAssert.AreEquivalent(testList, secondResult);

                cache.Remove(listStringCacheKey);
                List<string> thirdResult = await cache.GetAsync<List<string>>(listStringCacheKey, () => { return Task.FromResult(new List<string>()); });
                CollectionAssert.AreNotEquivalent(testList, thirdResult);
            }
        }
    }
}
