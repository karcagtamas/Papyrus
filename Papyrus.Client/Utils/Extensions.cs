using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Client.Utils;

public static class Extensions
{
    public static HttpQueryParameters AddNoteFilters(this HttpQueryParameters queryParams, NoteFilterQueryModel query)
    {
        return queryParams
            .Add("publicStatus", query.PublicStatus)
            .Add("archivedStatus", query.ArchivedStatus)
            .Add("textFilter", query.TextFilter)
            .Add("dateFilter", query.DateFilter)
            .AddMultiple("tags", query.Tags);
    }

    public static HttpQueryParameters AddSearchFilters(this HttpQueryParameters queryParams, SearchQueryModel query)
    {
        return queryParams
            .Add("text", query.Text)
            .Add("onlyPublics", query.OnlyPublics)
            .Add("includeContents", query.IncludeContents)
            .Add("includeTags", query.IncludeTags)
            .Add("startDate", query.StartDate)
            .Add("endDate", query.EndDate);
    }

    public static IObservable<T> ThrottleMax<T>(this IObservable<T> source, TimeSpan dueTime, TimeSpan maxTime) => source.ThrottleMax(dueTime, maxTime, Scheduler.Default);

    public static IObservable<T> ThrottleMax<T>(this IObservable<T> source, TimeSpan dueTime, TimeSpan maxTime, IScheduler scheduler)
    {
        return Observable.Create<T>(o =>
        {
            var hasValue = false;
            T value = default(T)!;

            var maxTimeDisposable = new SerialDisposable();
            var dueTimeDisposable = new SerialDisposable();

            Action action = () =>
            {
                if (hasValue)
                {
                    maxTimeDisposable.Disposable = Disposable.Empty;
                    dueTimeDisposable.Disposable = Disposable.Empty;
                    o.OnNext(value);
                    hasValue = false;
                }
            };

            return source.Subscribe(x =>
            {
                if (!hasValue)
                {
                    maxTimeDisposable.Disposable = scheduler.Schedule(maxTime, action);
                }

                hasValue = true;
                value = x;
                dueTimeDisposable.Disposable = scheduler.Schedule(dueTime, action);
            }, o.OnError, o.OnCompleted);
        });
    }
}
