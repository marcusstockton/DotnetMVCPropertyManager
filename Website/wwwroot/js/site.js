// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

(function () {
  const storageKey = 'theme';
  const attr = 'data-theme';

  function applyTheme(theme) {
    if (theme === 'dark') {
      document.documentElement.setAttribute(attr, 'dark');
    } else if (theme === 'light') {
      document.documentElement.removeAttribute(attr);
    } else {
      // follow system preference
      document.documentElement.removeAttribute(attr);
    }
  }

  function getSavedTheme() {
    return localStorage.getItem(storageKey);
  }

  function saveTheme(theme) {
    if (!theme) localStorage.removeItem(storageKey);
    else localStorage.setItem(storageKey, theme);
  }

  function updateDataTableButtons(theme) {
    if (typeof jQuery === 'undefined' || typeof jQuery.fn.dataTable === 'undefined') return;
    // style DataTables Buttons to use Bootstrap classes so CSS variables apply
    jQuery('.dataTables_wrapper .dt-buttons button.dt-button').each(function () {
      const $btn = jQuery(this);
      if (theme === 'dark') {
        $btn.removeClass('btn-light btn-secondary').addClass('btn btn-sm btn-primary');
      } else {
        $btn.removeClass('btn-primary').addClass('btn btn-sm btn-light');
      }
    });
  }

  // initialize
  const saved = getSavedTheme();
  if (saved) applyTheme(saved);

  // expose toggle for UI
  window.theme = {
    set(theme) { applyTheme(theme); saveTheme(theme); updateDataTableButtons(theme); },
    get() { return getSavedTheme(); },
    toggle() {
      const current = getSavedTheme();
      const next = current === 'dark' ? 'light' : 'dark';
      this.set(next);
    },
    // helper to notify that DataTables have been initialized on the page
    triggerDataTablesInitialized() {
      // dispatch event for any listeners
      try {
        document.dispatchEvent(new Event('datatables:initialized'));
      } catch (e) {
        // IE fallback
        const evt = document.createEvent('Event');
        evt.initEvent('datatables:initialized', true, true);
        document.dispatchEvent(evt);
      }
      // ensure buttons are styled immediately
      updateDataTableButtons(getSavedTheme());
    }
  };

  // alias for convenience
  window.triggerDataTablesInitialized = () => window.theme.triggerDataTablesInitialized();

  document.addEventListener('DOMContentLoaded', () => {
    // if there's a toggle button in the navbar, wire it
    const btn = document.getElementById('themeToggle');
    if (btn) {
      btn.addEventListener('click', () => {
        const current = getSavedTheme();
        const next = current === 'dark' ? 'light' : 'dark';
        theme.set(next);
        // update icon text if material icons used
        const icon = btn.querySelector('.material-icons');
        if (icon) icon.textContent = next === 'dark' ? 'dark_mode' : 'light_mode';
      });

      // set initial icon state
      const icon = btn.querySelector('.material-icons');
      if (icon) {
        const current = getSavedTheme();
        icon.textContent = current === 'dark' ? 'dark_mode' : 'light_mode';
      }
    }

    // apply correct DataTable button styles on load
    updateDataTableButtons(saved);

    // when DataTables are initialized later, ensure buttons are styled
    // listen for a custom event so pages can trigger it after DT init
    document.addEventListener('datatables:initialized', () => updateDataTableButtons(getSavedTheme()));

    // also observe DOM for DataTable buttons added later (fallback)
    if (window.MutationObserver) {
      const observer = new MutationObserver(() => updateDataTableButtons(getSavedTheme()));
      observer.observe(document.body, { childList: true, subtree: true });
    }
  });
})();