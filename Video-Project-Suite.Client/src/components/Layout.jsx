import React, { useState } from 'react';
import {
    AppBar,
    Box,
    Drawer,
    Fab,
    IconButton,
    List,
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Toolbar,
    Typography,
    useMediaQuery,
    useTheme
} from '@mui/material';
import { Menu, Work, People, Polyline, Flag } from '@mui/icons-material';
import { useNavigate, useLocation } from 'react-router-dom';
import Footer from './Footer';
import { useAuth } from '../context/AuthContext';

const drawerWidth = 240;

const Layout = ({ children }) => {
    const { isLoggedIn, logout } = useAuth();
    const [drawerOpen, setDrawerOpen] = useState(true);
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
    const navigate = useNavigate();
    const location = useLocation();

    const handleDrawerToggle = () => {
        setDrawerOpen(!drawerOpen);
    };

    const menuItems = [
        { text: 'Projects', icon: <Work />, path: '/projects' },
        { text: 'Users', icon: <People />, path: '/users' },
        { text: 'Project Assignments', icon: <Polyline />, path: '/user-project' },
        { text: 'Milestones', icon: <Flag />, path: '/milestones' },
    ];



    let authItems = [];

    if (!isLoggedIn) {
        authItems = [
            { text: 'Login', icon: <People />, path: '/login' },
            { text: 'Register', icon: <People />, path: '/register' },
        ];
    }

    const drawer = (
        <>
            <Toolbar>
                <Typography variant="h6" noWrap>
                    Video Project Suite
                </Typography>
            </Toolbar>
            {/* box display flex first list at top second list at bottom */}
            <Box sx={{ display: 'flex', flexDirection: 'column', height: '100%', justifyContent: 'space-between', marginBottom: 0 }}>
                {isLoggedIn && (
                    <List>
                        {menuItems.map((item) => (
                            <ListItem key={item.text} disablePadding>
                                <ListItemButton
                                    selected={location.pathname.startsWith(item.path)}
                                    onClick={() => navigate(item.path)}
                                >
                                    <ListItemIcon>{item.icon}</ListItemIcon>
                                    <ListItemText primary={item.text} />
                                </ListItemButton>
                            </ListItem>
                        ))}
                    </List>
                )}
                <List>
                    {authItems.map((item) => (
                        <ListItem key={item.text} disablePadding>
                            <ListItemButton
                                selected={location.pathname.startsWith(item.path)}
                                onClick={() => navigate(item.path)}
                            >
                                <ListItemIcon>{item.icon}</ListItemIcon>
                                <ListItemText primary={item.text} />
                            </ListItemButton>
                        </ListItem>
                    ))}
                    {isLoggedIn && (
                        <ListItem key='logout' disablePadding>
                            <ListItemButton
                                onClick={async () => {
                                    await logout();
                                    navigate('/login');
                                }}
                            >
                                <ListItemIcon><People /></ListItemIcon>
                                <ListItemText primary='Logout' />
                            </ListItemButton>
                        </ListItem>
                    )}
                </List>
            </Box>

        </>
    );

    return (
        <Box sx={{ display: 'flex' }}>
            <AppBar
                position="fixed"
                sx={{
                    width: drawerOpen ? { sm: `calc(100% - ${drawerWidth}px)` } : '100%',
                    ml: drawerOpen ? { sm: `${drawerWidth}px` } : 0,
                    transition: theme.transitions.create(['margin', 'width'], {
                        easing: theme.transitions.easing.sharp,
                        duration: theme.transitions.duration.leavingScreen,
                    }),
                }}
            >
                <Toolbar>
                    <IconButton
                        color="inherit"
                        edge="start"
                        onClick={handleDrawerToggle}
                        sx={{ mr: 2 }}
                    >
                        <Menu />
                    </IconButton>
                    <Typography variant="h6" noWrap>
                        {menuItems.find(item => location.pathname.startsWith(item.path))?.text || 'Projects'}
                    </Typography>
                </Toolbar>
            </AppBar>

            <Box
                component="nav"
                sx={{ width: drawerOpen ? { sm: drawerWidth } : 0, flexShrink: { sm: 0 } }}
            >
                {/* Mobile: temporary drawer */}
                <Drawer
                    variant="temporary"
                    open={drawerOpen}
                    onClose={handleDrawerToggle}
                    ModalProps={{ keepMounted: true }}
                    sx={{
                        display: { xs: 'block', sm: 'none' },
                        '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
                    }}
                >
                    {drawer}
                </Drawer>
                {/* Desktop: persistent drawer */}
                <Drawer
                    variant="persistent"
                    sx={{
                        display: { xs: 'none', sm: 'block' },
                        '& .MuiDrawer-paper': {
                            boxSizing: 'border-box',
                            width: drawerWidth,
                            transition: theme.transitions.create('width', {
                                easing: theme.transitions.easing.sharp,
                                duration: theme.transitions.duration.enteringScreen,
                            }),
                        },
                    }}
                    open={drawerOpen}
                >
                    {drawer}
                </Drawer>
            </Box>

            <Box
                component="main"
                sx={{
                    flexGrow: 1,
                    p: 3,
                    width: drawerOpen ? { sm: `calc(100% - ${drawerWidth}px)` } : '100%',
                    transition: theme.transitions.create(['margin', 'width'], {
                        easing: theme.transitions.easing.sharp,
                        duration: theme.transitions.duration.leavingScreen,
                    }),
                }}
            >
                <Toolbar />
                {children}
                <Footer />
            </Box>
        </Box>
    );
};

export default Layout;