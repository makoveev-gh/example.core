using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Core.Contracts;

public interface IInitializable
{
	bool IsInitialized { get; }

	Task InitializeAsync(CancellationToken cancellationToken);
}
