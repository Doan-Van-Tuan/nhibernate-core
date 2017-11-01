﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NUnit.Framework;

namespace NHibernate.Test.GenericTest.EnumGeneric
{
	using System.Threading.Tasks;
	/// <summary>
	/// http://nhibernate.jira.com/browse/NH-1236
	/// </summary>
	[TestFixture]
	public class EnumGenericFixtureAsync : TestCase
	{
		protected override IList Mappings
		{
			get { return new String[] {"GenericTest.EnumGeneric.EnumGenericFixture.hbm.xml"}; }
		}

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		[Test]
		public async Task PersistsAsync()
		{
			A a1 = new A();

			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(a1));
				await (s.FlushAsync());
			}

			//Verify initial null
			using (ISession s = OpenSession())
			{
				A a2 = await (s.LoadAsync<A>(a1.Id));
				Assert.IsNull(a2.NullableValue);
				a2.NullableValue = B.Value3;
				await (s.SaveAsync(a2));
				await (s.FlushAsync());
			}

			//Verify set to non-null
			using (ISession s = OpenSession())
			{
				A a3 = await (s.LoadAsync<A>(a1.Id));
				Assert.AreEqual(B.Value3, a3.NullableValue);
				a3.NullableValue = null;
				await (s.SaveAsync(a3));
				await (s.FlushAsync());
			}

			//Verify set to null
			using (ISession s = OpenSession())
			{
				A a4 = await (s.LoadAsync<A>(a1.Id));
				Assert.IsNull(a4.NullableValue);
				await (s.DeleteAsync(a4));
				await (s.FlushAsync());
			}
		}
	}
}