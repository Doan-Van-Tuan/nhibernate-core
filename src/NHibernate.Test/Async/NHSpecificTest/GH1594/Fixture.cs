﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Linq;
using System.Threading;
using System.Transactions;
using NHibernate.Engine;
using NUnit.Framework;
using NHibernate.Linq;

namespace NHibernate.Test.NHSpecificTest.GH1594
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override bool AppliesTo(ISessionFactoryImplementor factory) =>
			factory.ConnectionProvider.Driver.SupportsSystemTransactions;

		protected override void OnSetUp()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				var e1 = new GH1594.Entity {Name = "Bob"};
				session.Save(e1);

				var e2 = new GH1594.Entity {Name = "Sally"};
				session.Save(e2);

				transaction.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				// The HQL delete does all the job inside the database without loading the entities, but it does
				// not handle delete order for avoiding violating constraints if any. Use
				// session.Delete("from System.Object");
				// instead if in need of having NHbernate ordering the deletes, but this will cause
				// loading the entities in the session.
				session.CreateQuery("delete from System.Object").ExecuteUpdate();

				transaction.Commit();
			}
		}

		[Test]
		public async Task ExecutionContextLocalValuesLeakAsync()
		{
			using (var session = OpenSession())
			{
				await (RunInTransactionAsync(session));
				var localValuesCountAfterFirstCall = ExecutionContext.Capture().LocalValuesCount();
				await (RunInTransactionAsync(session));
				var localValuesCountAfterSecondCall = ExecutionContext.Capture().LocalValuesCount();
				Assert.AreEqual(localValuesCountAfterFirstCall, localValuesCountAfterSecondCall);
			}
		}

		private async Task RunInTransactionAsync(ISession session, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				var result = from e in session.Query<GH1594.Entity>()
							 where e.Name == "Bob"
							 select e;

				await (result.ToListAsync(cancellationToken));
				ts.Complete();
			}
		}
	}
}
