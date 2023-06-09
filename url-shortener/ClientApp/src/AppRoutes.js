import { Home } from "./components/Home";
import Login from "./components/Auth/Login";
import Register from "./components/Auth/Register";
import ShortUrlRedirect from "./components/ShortUrl/ShortUrlRedirect";
import ShortUrls from "./components/ShortUrl/ShortUrl";
import Info from "./components/ShortUrl/Info";
import About from "./components/About/About";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/login',
    element: <Login />
  },
  {
    path: '/register',
    element: <Register />
  },
  {
    path: '/short-urls',
    element: <ShortUrls />
  },
  {
    path: `/info/:id`,
    element: <Info />
  },
  {
    path: `/redirect/:id`,
    element: <ShortUrlRedirect />
  },
  {
    path: `/about`,
    element: <About />
  }
];

export default AppRoutes;
