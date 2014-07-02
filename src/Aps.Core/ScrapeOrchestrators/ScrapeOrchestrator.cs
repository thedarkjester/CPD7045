namespace Aps.Scheduling.ApplicationService.ScrapeOrchestrators
{
    public abstract class ScrapeOrchestrator
    {
        public abstract void Orchestrate(Entities.ScrapeOrchestratorEntity scrapeOrchestratorEntity);        
    }
}
