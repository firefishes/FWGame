namespace ShipDock.Notices
{
    public class ParamNotice<T> : Notice
    {
        protected override void Purge()
        {
            base.Purge();

            ParamValue = default;
        }

        public T ParamValue { get; set; }
    }
}
